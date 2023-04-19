using Microsoft.AspNetCore.Connections;
using System.Buffers.Text;
using System.Data;
using System.Numerics;
using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography.Xml;
using System.Diagnostics;
using System.Transactions;

namespace Capstone.DAO
{
    public class TradeSqlDao : ITradeDao
    {
        private static ICardDao cardDao;
        private static ICollectionDao collectionDao;
        private readonly string connectionString;

        public TradeSqlDao(string dbConnectionString, ICardDao _cardDao, ICollectionDao _collectionDao)
        {
            connectionString = dbConnectionString;
            cardDao = _cardDao;
            collectionDao = _collectionDao;
        }

        public void AddTrade(Trade trade)
        {
            int tradeId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //See if there is already a trade between the active users
                try
                {
                    SqlCommand isTradeAlreadyActive = new SqlCommand("SELECT * FROM trade WHERE (user_id_from = (SELECT user_id FROM users WHERE username = @userOne) " +
                                                                     "AND user_id_to = (SELECT user_id FROM users WHERE username = @userTwo)) OR " +
                                                                     "(user_id_from = (SELECT user_id FROM users WHERE username = @userTwo) " +
                                                                     "AND user_id_to = (SELECT user_id FROM users WHERE username = @userOne)) " +
                                                                     "AND trade.status = 'pending'", conn);
                    isTradeAlreadyActive.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                    isTradeAlreadyActive.Parameters.AddWithValue("@userTwo", trade.UsernameTo);
                    SqlDataReader hasOpenTrade = isTradeAlreadyActive.ExecuteReader();
                    if (hasOpenTrade.Read())
                    {
                        throw new Exception("Trade between these users is already active");
                    }
                    hasOpenTrade.Close();
                }
                catch (Exception ex)
                {
                   throw new Exception ($"Error checking if trade is already active. Sql Exception: '{ex.Message}'");
                }
                    

                //Add trade to trade table
                try
                {
                    SqlCommand addToTradeTable = new SqlCommand("INSERT INTO trade (user_id_from, user_id_to) " +
                                                            "OUTPUT INSERTED.trade_id " +
                                                            "VALUES " +
                                                            "((SELECT user_id FROM users WHERE username = @userOne), " +
                                                            "(SELECT user_id FROM users WHERE username = @userTwo));", conn);
                    addToTradeTable.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                    addToTradeTable.Parameters.AddWithValue("@userTwo", trade.UsernameTo);

                    tradeId = Convert.ToInt32(addToTradeTable.ExecuteNonQuery());
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding trade to trade table. Sql Exception: '{ex.Message}'");
                }

                //Add collection Items From to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsFrom)
                {
                    try
                    {
                        SqlCommand addCollectionItemsFrom = new SqlCommand("INSERT INTO trade_card_collection (trade_id, id, collection_id) VALUES (@tradeId," +
                                                                       "(SELECT id FROM card where name = @cardName)," +
                                                                       "(SELECT collection_id FROM collection WHERE user_id = " +
                                                                       "(SELECT user_id from users WHERE username = @username)));", conn);
                        addCollectionItemsFrom.Parameters.AddWithValue("@tradeId", tradeId);
                        addCollectionItemsFrom.Parameters.AddWithValue("@cardName", item.Card.Name);
                        addCollectionItemsFrom.Parameters.AddWithValue("@username", trade.UsernameFrom);

                        addCollectionItemsFrom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error adding collection to each side of the trade with card '{item.Card.Name}' owned by '{trade.UsernameFrom}'. Sql Exception: '{ex.Message}'");
                    }
                }

                //Add collection Items To to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsTo)
                {
                    try
                    {
                        SqlCommand addCollectionItemsFrom = new SqlCommand("INSERT INTO trade_card_collection (trade_id, id, collection_id) VALUES (@tradeId," +
                                                                       "(SELECT id FROM card where name = @cardName)," +
                                                                       "(SELECT collection_id FROM collection WHERE user_id = " +
                                                                       "(SELECT user_id from users WHERE username = @username)));", conn);
                        addCollectionItemsFrom.Parameters.AddWithValue("@tradeId", tradeId);
                        addCollectionItemsFrom.Parameters.AddWithValue("@cardName", item.Card.Name);
                        addCollectionItemsFrom.Parameters.AddWithValue("@username", trade.UsernameTo);

                        addCollectionItemsFrom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error adding collection to each side of the trade with card '{item.Card.Name}' owned by '{trade.UsernameTo}'. Sql Exception: '{ex.Message}'");
                    }
                }
            }
        }

        public Trade GetTrade(string userOne, string userTwo)
        {
            Trade trade = new Trade();
            int collectionIdFrom = 0;
            int collectionIdTo = 0;
            List<string> cardIdsFrom;
            List<string> cardIdsTo;
            List<CollectionItem> collectionItemsFrom;
            List<CollectionItem> collectionItemsTo;

            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();
                try
                {
                    //Getting the non-join table based trade attributes
                    trade = LocateTradeFromUsernames(conn, userOne, userTwo);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error finding trade in the database. Sql Exception: '{ex.Message}'");
                }

                //Getting the collectionIds of the users involved in the trade
                try
                {
                    collectionIdFrom = GetCollectionIdFromUsername(conn, userOne);

                    collectionIdTo = GetCollectionIdFromUsername(conn, userTwo);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error finding collection IDs of corresponding users. Sql Exception: '{ex.Message}'");
                }

                //Getting the rows of the Join table for Trade_card_Collection (User From)
                try
                {
                    SqlCommand getCardIdsFrom = new SqlCommand("SELECT * FROM trade_card_collection WHERE trade_id = @tradeId AND collection_id = @collectionId;", conn);
                    getCardIdsFrom.Parameters.AddWithValue("@tradeId", trade.TradeId);
                    getCardIdsFrom.Parameters.AddWithValue("@collectionId", collectionIdFrom);
                    cardIdsFrom = new List<string>();
                    SqlDataReader getCardIdsFromReader = getCardIdsFrom.ExecuteReader();
                    while (getCardIdsFromReader.Read())
                    {
                        string cardId = Convert.ToString(getCardIdsFromReader["id"]);
                        cardIdsFrom.Add(cardId);
                    }
                    getCardIdsFromReader.Close();
                }
                catch (Exception Ex)
                {
                    throw new Exception($"Error find cards from user '{userOne}'. Sql Exception: '{Ex.Message}'");
                }

                //Getting the rows of the Join table for Trade_card_Collection (User From)
                try
                {
                    SqlCommand getCardIdsTo = new SqlCommand("SELECT * FROM trade_card_collection WHERE trade_id = @tradeId AND collection_id = @collectionId;", conn);
                    getCardIdsTo.Parameters.AddWithValue("@tradeId", trade.TradeId);
                    getCardIdsTo.Parameters.AddWithValue("@collectionId", collectionIdTo);
                    cardIdsTo = new List<string>();
                    SqlDataReader getCardIdsToReader = getCardIdsTo.ExecuteReader();
                    while (getCardIdsToReader.Read())
                    {
                        string cardId = Convert.ToString(getCardIdsToReader["id"]);
                        cardIdsTo.Add(cardId);
                    }
                    getCardIdsToReader.Close();
                }
                catch (Exception Ex)
                {
                    throw new Exception($"Error find card IDs from user '{userTwo}'. Sql Exception: '{Ex.Message}'");
                }


                //Getting collectionItemsFrom
                try
                {
                    collectionItemsFrom = new List<CollectionItem>();
                    foreach (string cardId in cardIdsFrom)
                    {
                        SqlCommand getCollectionItemInfo = new SqlCommand("SELECT card.id, card.name, card.img, card.price, card.low_price, card.high_price, card.rarity, card.tcg_url, collection_card.quantity, collection_card.amount_to_trade, collection_card.grade FROM card" +
                                                                          " JOIN collection_card ON card.id = collection_card.id" +
                                                                          " WHERE collection_id = @collectionId AND card.id = @cardId;", conn);

                        getCollectionItemInfo.Parameters.AddWithValue("@collectionId", collectionIdFrom);
                        getCollectionItemInfo.Parameters.AddWithValue("@cardId", cardId);

                        SqlDataReader reader6 = getCollectionItemInfo.ExecuteReader();

                        if (reader6.Read())
                        {
                            Card newCard = new Card();
                            newCard = cardDao.getCardFromReader(reader6);
                            CollectionItem item = collectionDao.getCollectionItemFromReader(reader6, newCard);
                            collectionItemsFrom.Add(item);
                        }
                        reader6.Close();
                    }
                    trade.CollectionItemsFrom = collectionItemsFrom;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error find collection item information from user '{userOne}'. Sql Exception: '{ex.Message}'");
                }
               

                //Getting CollectionItemsTo
                try
                {
                    collectionItemsTo = new List<CollectionItem>();
                    foreach (string cardId in cardIdsTo)
                    {
                        SqlCommand getCollectionItemInfo = new SqlCommand("SELECT card.id, card.name, card.img, card.price, card.low_price, card.high_price, card.rarity, card.tcg_url, collection_card.quantity, collection_card.amount_to_trade, collection_card.grade FROM card" +
                                                                          " JOIN collection_card ON card.id = collection_card.id" +
                                                                          " WHERE collection_id = @collectionId AND card.id = @cardId;", conn);

                        getCollectionItemInfo.Parameters.AddWithValue("@collectionId", collectionIdTo);
                        getCollectionItemInfo.Parameters.AddWithValue("@cardId", cardId);

                        SqlDataReader reader7 = getCollectionItemInfo.ExecuteReader();

                        if (reader7.Read())
                        {
                            Card newCard = new Card();
                            newCard = cardDao.getCardFromReader(reader7);
                            CollectionItem item = collectionDao.getCollectionItemFromReader(reader7, newCard);
                            collectionItemsTo.Add(item);
                        }
                        reader7.Close();
                    }
                    trade.CollectionItemsTo = collectionItemsTo;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error find collection item information from user '{userTwo}'. Sql Exception: '{ex.Message}'");
                }
            }
            return trade;
        }

        public void AcceptTrade(string userOne, string userTwo)
        {
            Trade trade = GetTrade(userOne, userTwo);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Begin transaction
                conn.Open();
                int collectionIdFrom = GetCollectionIdFromUsername(conn, userOne);
                int collectionIdTo = GetCollectionIdFromUsername(conn, userTwo);
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {

                    //Remove one copy of all cards in userOne's
                    foreach (CollectionItem item in trade.CollectionItemsFrom)
                    {
                        string cardId = item.Card.Id;

                        SqlCommand removeCardsFromUserFrom = new SqlCommand("UPDATE collection_card SET quantity -= 1, amount_to_trade -= 1 " +
                                                                            "WHERE collection_id = @collectionId AND " +
                                                                            "id = @cardId;", conn);
                        removeCardsFromUserFrom.Parameters.AddWithValue("@collectionId", collectionIdFrom);
                        removeCardsFromUserFrom.Parameters.AddWithValue("@cardId", cardId);
                        removeCardsFromUserFrom.Transaction = transaction;
                        removeCardsFromUserFrom.ExecuteNonQuery();
                    }

                    //Remove one copy of all cards in userTwo's
                    foreach (CollectionItem item in trade.CollectionItemsTo)
                    {
                        string cardId = item.Card.Id;

                        SqlCommand removeCardsFromUserTo = new SqlCommand("UPDATE collection_card SET quantity -= 1, amount_to_trade -= 1 " +
                                                                            "WHERE collection_id = @collectionId AND " +
                                                                            "id = @cardId;", conn);
                        removeCardsFromUserTo.Parameters.AddWithValue("@collectionId", collectionIdTo);
                        removeCardsFromUserTo.Parameters.AddWithValue("@cardId", cardId);
                        removeCardsFromUserTo.Transaction = transaction;
                        removeCardsFromUserTo.ExecuteNonQuery();
                    }

                    //Deleteing Empty Entries
                    SqlCommand deleteEmptyCollectionItems = new SqlCommand("DELETE collection_card  WHERE quantity = 0;", conn);
                    deleteEmptyCollectionItems.Transaction = transaction;
                    deleteEmptyCollectionItems.ExecuteNonQuery();

                    //Adding removed from userOne's to UserTwo's
                    foreach (CollectionItem item in trade.CollectionItemsFrom)
                    {
                        string cardId = item.Card.Id;
                        AddCollectionItemToCollection(item, trade.UsernameTo, conn, transaction);
                    }

                    //Adding removed from userOne's to UserTwo's
                    foreach (CollectionItem item in trade.CollectionItemsTo)
                    {
                        string cardId = item.Card.Id;
                        AddCollectionItemToCollection(item, trade.UsernameFrom, conn, transaction);
                    }
                    //Change status to accepted
                    SqlCommand acceptTrade = new SqlCommand("UPDATE trade SET status = 'accepted' " +
                                                      "WHERE user_id_from = " +
                                                      "(SELECT user_id FROM users WHERE username = @userOne) AND " +
                                                      "user_id_to = " +
                                                      "(SELECT user_id FROM users WHERE username = @userTwo) OR " +
                                                      "user_id_from = " +
                                                      "(SELECT user_id FROM users WHERE username = @userTwo) AND " +
                                                      "user_id_to = " +
                                                      "(SELECT user_id FROM users WHERE username = @userOne) " +
                                                      "AND trade.status = 'pending';", conn);
                    acceptTrade.Parameters.AddWithValue("@userOne", userOne);
                    acceptTrade.Parameters.AddWithValue("@userTwo", userTwo);
                    acceptTrade.Transaction = transaction;
                    acceptTrade.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        
        public void RejectTrade(string userOne, string userTwo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand rejectTrade = new SqlCommand("UPDATE trade SET status = 'rejected' " +
                                                     "WHERE user_id_from = " +
                                                     "(SELECT user_id FROM users WHERE username = @userOne) AND " +
                                                     "user_id_to = " +
                                                     "(SELECT user_id FROM users WHERE username = @userTwo) OR " +
                                                     "user_id_from = " +
                                                     "(SELECT user_id FROM users WHERE username = @userTwo) AND " +
                                                     "user_id_to = " +
                                                     "(SELECT user_id FROM users WHERE username = @userOne) " +
                                                     "AND trade.status = 'pending';", conn);
                rejectTrade.Parameters.AddWithValue("@userOne", userOne);
                rejectTrade.Parameters.AddWithValue("@userTwo", userTwo);
                rejectTrade.ExecuteNonQuery();
            }
        }

        private int GetCollectionIdFromUsername(SqlConnection conn, string username)
        {
            int collectionId = 0;
            SqlCommand getCollectionId = new SqlCommand("SELECT collection_id FROM collection WHERE user_id = (SELECT user_id FROM users WHERE username = @username);", conn);
            getCollectionId.Parameters.AddWithValue("@username", username);

            SqlDataReader getCollectionIdFromReader = getCollectionId.ExecuteReader();
            if (getCollectionIdFromReader.Read())
            {
                collectionId = Convert.ToInt32(getCollectionIdFromReader["collection_id"]);
            }
            getCollectionIdFromReader.Close();
            if (collectionId == 0)
            {
                throw new Exception("Could not locate collectionID.");
            }
            return collectionId;
        }

        private Trade LocateTradeFromUsernames(SqlConnection conn, string userOne, string userTwo) 
        {
            Trade trade = new Trade();
            SqlCommand getTrade = new SqlCommand("SELECT * FROM trade " +
                                                             "WHERE user_id_from = " +
                                                             "(SELECT user_id FROM users WHERE username = @userOne) AND " +
                                                             "user_id_to = " +
                                                             "(SELECT user_id FROM users WHERE username = @userTwo) OR " +
                                                             "user_id_from = " +
                                                             "(SELECT user_id FROM users WHERE username = @userTwo) AND " +
                                                             "user_id_to = " +
                                                             "(SELECT user_id FROM users WHERE username = @userOne) " +
                                                             "AND trade.status = 'pending';", conn);
            getTrade.Parameters.AddWithValue("@userOne", userOne);
            getTrade.Parameters.AddWithValue("@userTwo", userTwo);

            SqlDataReader getTradeReader = getTrade.ExecuteReader();
            if (getTradeReader.Read())
            {
                trade = CreateTradeFromReader(getTradeReader, userOne, userTwo);
            }
            getTradeReader.Close();

            if (trade.Status != "pending")
            {
                throw new Exception("Could not locate trade");
            }
            return trade;
        }

        private Trade CreateTradeFromReader(SqlDataReader reader,string UserOne, string UserTwo)
        {
            Trade trade = new Trade();
            trade.TradeId = Convert.ToInt32(reader["trade_id"]);
            trade.UsernameFrom = UserOne;
            trade.UsernameTo = UserTwo;
            trade.Status = Convert.ToString(reader["status"]);
            return trade;
        }

        public void AddCollectionItemToCollection(CollectionItem collectionItem, string username, SqlConnection conn, SqlTransaction transaction)
        {
            collectionItem.Quantity = 1;
            collectionItem.QuantityForTrade = 0;
            SqlCommand findCardInDB = new SqlCommand("SELECT * FROM card WHERE id = @id", conn);
            findCardInDB.Parameters.AddWithValue("@id", collectionItem.Card.Id);
            findCardInDB.Transaction = transaction;
            bool cardExists = (findCardInDB.ExecuteScalar() != null ? true : false);

            SqlCommand findCardInCollection = new SqlCommand("SELECT * FROM collection_card WHERE id = @id AND " +
                                                            "collection_id = (SELECT collection_id FROM collection WHERE user_id = " +
                                                            "(SELECT user_id FROM users WHERE username = @username)); ", conn);
            findCardInCollection.Parameters.AddWithValue("@id", collectionItem.Card.Id);
            findCardInCollection.Parameters.AddWithValue("@username", username);
            findCardInCollection.Transaction = transaction;
            bool isInCollection = (findCardInCollection.ExecuteScalar() != null ? true : false);

            if (!cardExists)
            {
                SqlCommand addCardToCardTable = new SqlCommand("INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES (@name, @id, @img, @price, @low_price, @high_price, @rarity, @tcg_url);", conn);
                addCardToCardTable.Parameters.AddWithValue("@name", collectionItem.Card.Name);
                addCardToCardTable.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                addCardToCardTable.Parameters.AddWithValue("@img", collectionItem.Card.Img);
                addCardToCardTable.Parameters.AddWithValue("@price", collectionItem.Card.Price);
                addCardToCardTable.Parameters.AddWithValue("@low_price", collectionItem.Card.LowPrice);
                addCardToCardTable.Parameters.AddWithValue("@high_price", collectionItem.Card.HighPrice);
                addCardToCardTable.Parameters.AddWithValue("@rarity", collectionItem.Card.Rarity);
                addCardToCardTable.Parameters.AddWithValue("@tcg_url", collectionItem.Card.TcgUrl);
                addCardToCardTable.Transaction = transaction;
                int numCardRowsAffected = addCardToCardTable.ExecuteNonQuery();
                if (numCardRowsAffected != 1)
                {
                    throw new Exception("Card Wasn't added to database properly");
                }
                SqlCommand addCardToCollection = new SqlCommand("INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) " +
                                                            "OUTPUT INSERTED.id " +
                                                            "VALUES ((SELECT collection_id FROM collection WHERE user_id = " +
                                                            "(SELECT user_id FROM users WHERE username = @username)), " +
                                                            "@id, @quantity, @amount_to_trade) ", conn);
                addCardToCollection.Parameters.AddWithValue("@username", username);
                addCardToCollection.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                addCardToCollection.Parameters.AddWithValue("@quantity", collectionItem.Quantity);
                addCardToCollection.Parameters.AddWithValue("@amount_to_trade", collectionItem.QuantityForTrade);
                addCardToCollection.Transaction = transaction;

                string outputId = Convert.ToString(addCardToCollection.ExecuteScalar());
                if (outputId != collectionItem.Card.Id)
                {
                    throw new Exception("Card wasn't added to collection properly");
                }
            }
            else
            {
                if (!isInCollection)
                {
                    SqlCommand addCardToCollection = new SqlCommand("INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) " +
                                                            "OUTPUT INSERTED.id " +
                                                            "VALUES ((SELECT collection_id FROM collection WHERE user_id = " +
                                                            "(SELECT user_id FROM users WHERE username = @username)), " +
                                                            "@id, @quantity, @amount_to_trade) ", conn);
                    addCardToCollection.Parameters.AddWithValue("@username", username);
                    addCardToCollection.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                    addCardToCollection.Parameters.AddWithValue("@quantity", collectionItem.Quantity);
                    addCardToCollection.Parameters.AddWithValue("@amount_to_trade", collectionItem.QuantityForTrade);
                    addCardToCollection.Transaction = transaction;

                    string outputId = Convert.ToString(addCardToCollection.ExecuteScalar());
                    if (outputId != collectionItem.Card.Id)
                    {
                        throw new Exception("Card wasn't added to collection properly");
                    }
                }
                else
                {
                    SqlCommand updateCardValue = new SqlCommand("UPDATE collection_card SET quantity += @quantity " +
                                                            "WHERE id = @id AND " +
                                                            "collection_id = (SELECT collection_id FROM collection WHERE user_id = " +
                                                            "(SELECT user_id FROM users WHERE username = @username)); ", conn);
                    updateCardValue.Parameters.AddWithValue("@quantity", collectionItem.Quantity);
                    updateCardValue.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                    updateCardValue.Parameters.AddWithValue("@username", username);
                    updateCardValue.Transaction = transaction;
                    int numCardRowsAffected = updateCardValue.ExecuteNonQuery();
                    if (numCardRowsAffected != 1)
                    {
                        throw new Exception("I don't know why this is breaking");
                    }
                }
            }
        }
    }
}
