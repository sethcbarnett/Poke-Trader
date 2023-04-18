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

        public string AddTrade(Trade trade)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //See if there is already a trade between the active users

                SqlCommand isTradeAlreadyActive = new SqlCommand("SELECT * FROM trade WHERE (user_id_from = (SELECT user_id FROM users WHERE username = @userOne) " +
                                                                 "AND user_id_to = (SELECT user_id FROM users WHERE username = @userTwo)) OR " +
                                                                 "(user_id_from = (SELECT user_id FROM users WHERE username = @userTwo) " +
                                                                 "AND user_id_to = (SELECT user_id FROM users WHERE username = @userOne)) ", conn);
                isTradeAlreadyActive.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                isTradeAlreadyActive.Parameters.AddWithValue("@userTwo", trade.UsernameTo);
                SqlDataReader hasOpenTrade = isTradeAlreadyActive.ExecuteReader();
                if (hasOpenTrade.Read())
                {
                    return "These users already have an open trade.";
                }
                hasOpenTrade.Close();

                //Add trade to trade table
                SqlCommand addToTradeTable = new SqlCommand("INSERT INTO trade (user_id_from, user_id_to) " +
                                                            "OUTPUT INSERTED.trade_id " +
                                                            "VALUES " +
                                                            "((SELECT user_id FROM users WHERE username = @userOne), " +
                                                            "(SELECT user_id FROM users WHERE username = @userTwo));", conn);
                addToTradeTable.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                addToTradeTable.Parameters.AddWithValue("@userTwo", trade.UsernameTo);

                int tradeId = Convert.ToInt32(addToTradeTable.ExecuteNonQuery());

                //Add collection Items From to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsFrom)
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

                //Add collection Items To to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsTo)
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
            }
            return "Hooray";
        }

        public Trade GetTrade(string userOne, string userTwo)
        {
            Trade trade = new Trade();
            int collectionIdFrom = 0;
            int collectionIdTo = 0;
            int tradeId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();

                //Getting the non-join table based trade attributes
                SqlCommand getTrade = new SqlCommand("SELECT * FROM trade " +
                                                     "WHERE user_id_from = " +
                                                     "(SELECT user_id FROM users WHERE username = @userOne) AND " +
                                                     "user_id_to = " +
                                                     "(SELECT user_id FROM users WHERE username = @userTwo) OR " +
                                                     "user_id_from = " +
                                                     "(SELECT user_id FROM users WHERE username = @userTwo) AND " +
                                                     "user_id_to = " +
                                                     "(SELECT user_id FROM users WHERE username = @userOne); ", conn);
                getTrade.Parameters.AddWithValue("@userOne", userOne);
                getTrade.Parameters.AddWithValue("@userTwo", userTwo);

                SqlDataReader reader = getTrade.ExecuteReader();
                if(reader.Read())
                {
                    trade = CreateTradeFromReader(reader, userOne, userTwo);
                    tradeId = trade.TradeId;
                }

                SqlCommand getCollectionIdFrom = new SqlCommand("SELECT collection_id FROM collection WHERE user_id = (SELECT user_id FROM users WHERE username = @username);", conn);
                reader.Close();

                //Getting the collectionIds of the users involved in the trade
                getCollectionIdFrom.Parameters.AddWithValue("@username", userOne);

                SqlDataReader reader2 = getCollectionIdFrom.ExecuteReader();
                if (reader2.Read())
                {
                    collectionIdFrom = Convert.ToInt32(reader2["collection_id"]);
                }
                reader2.Close();

                SqlCommand getCollectionIdTo = new SqlCommand("SELECT collection_id FROM collection WHERE user_id = (SELECT user_id FROM users WHERE username = @username);", conn);

                getCollectionIdTo.Parameters.AddWithValue("@username", userTwo);
                

                SqlDataReader reader3 = getCollectionIdTo.ExecuteReader();
                if (reader3.Read())
                {
                    collectionIdTo = Convert.ToInt32(reader3["collection_id"]);
                }
                reader3.Close();
                //Getting the rows of the Join table for Trade_card_Collection (User From)
                SqlCommand getCardIdsFrom = new SqlCommand("SELECT * FROM trade_card_collection WHERE trade_id = @tradeId AND collection_id = @collectionId;", conn);
                getCardIdsFrom.Parameters.AddWithValue("@tradeId", tradeId);
                getCardIdsFrom.Parameters.AddWithValue("@collectionId", collectionIdFrom);
                List<string> cardIdsFrom = new List<string>();
                SqlDataReader reader4 = getCardIdsFrom.ExecuteReader();
                while (reader4.Read())
                {
                    string cardId = Convert.ToString(reader4["id"]);
                    cardIdsFrom.Add(cardId);
                }
                reader4.Close();

                //Getting the rows of the Join table for Trade_card_Collection (User From)
                SqlCommand getCardIdsTo = new SqlCommand("SELECT * FROM trade_card_collection WHERE trade_id = @tradeId AND collection_id = @collectionId;", conn);
                getCardIdsTo.Parameters.AddWithValue("@tradeId", tradeId);
                getCardIdsTo.Parameters.AddWithValue("@collectionId", collectionIdTo);
                List<string> cardIdsTo = new List<string>();
                SqlDataReader reader5 = getCardIdsTo.ExecuteReader();
                while (reader5.Read())
                {
                    string cardId = Convert.ToString(reader5["id"]);
                    cardIdsTo.Add(cardId);
                }
                reader5.Close();

                //Getting collectionItemsFrom
                List<CollectionItem> collectionItemsFrom = new List<CollectionItem>();

                foreach(string cardId in cardIdsFrom)
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

                //Getting CollectionItemsTo
                List<CollectionItem> collectionItemsTo = new List<CollectionItem>();

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
            return trade;
        }

        public Trade AttemptTrade(Trade trade)
        {
            return trade;
        }
        public void UpdateTrade(Trade trade)
        {

        }

        public void DeleteTrade(int tradeId)
        {

        }
        public List<Trade> GetTradeList(int userId)
        {
            return null;
        }

        public void AcceptTrade(Trade trade)
        {
        }
        
        public void RejectTrade(Trade trade)
        {
        }

        public Trade FulfillTrade(Trade trade)
        {
            return null;
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

        public SqlTradeJoinObject CreateSQLTJOFromReader(SqlDataReader reader)
        {
            SqlTradeJoinObject sqltjo = new SqlTradeJoinObject();
            sqltjo.collectionId = Convert.ToInt32(reader["collection_id"]);
            sqltjo.cardId = Convert.ToString(reader["id"]);
            return sqltjo;
        }

    }
}
