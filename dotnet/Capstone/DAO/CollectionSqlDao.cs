using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Capstone.DAO
{
    public class CollectionSqlDao : ICollectionDao
    {
        private static ICardDao cardDao;
        public string connectionString;
        public CollectionSqlDao(string dbConnectionString, ICardDao _cardDao)
        {
            connectionString = dbConnectionString;
            cardDao = _cardDao;
        }

        public CollectionItem AddCollectionItemToCollection(CollectionItem collectionItem, string username)
        {
            CollectionItem databaseItem = null;
            //try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand findCardInDB = new SqlCommand("SELECT * FROM card WHERE id = @id", conn);
                    findCardInDB.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                    bool cardExists = (findCardInDB.ExecuteScalar() != null ? true : false);

                    SqlCommand findCardInCollection = new SqlCommand("SELECT * FROM collection_card WHERE id = @id AND " +
                                                                    "collection_id = (SELECT collection_id FROM collection WHERE user_id = " +
                                                                    "(SELECT user_id FROM users WHERE username = @username)); ", conn);
                    findCardInCollection.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                    findCardInCollection.Parameters.AddWithValue("@username", username);
                    bool isInCollection = (findCardInCollection.ExecuteScalar() != null ? true : false);

                    if (!cardExists)
                    {
                        SqlCommand addCardToCardTable = new SqlCommand("INSERT INTO card (name, id, img, price, tcg_url) VALUES (@name, @id, @img, @price, @tcg_url);", conn);
                        addCardToCardTable.Parameters.AddWithValue("@name", collectionItem.Card.Name);
                        addCardToCardTable.Parameters.AddWithValue("@id", collectionItem.Card.Id);
                        addCardToCardTable.Parameters.AddWithValue("@img", collectionItem.Card.Img);
                        addCardToCardTable.Parameters.AddWithValue("@price", collectionItem.Card.Price);
                        addCardToCardTable.Parameters.AddWithValue("@tcg_url", collectionItem.Card.TcgUrl);
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
                            int numCardRowsAffected = updateCardValue.ExecuteNonQuery();
                            if (numCardRowsAffected != 1)
                            {
                                throw new Exception("I don't know why this is breaking");
                            }
                        }
                    }
                }
                databaseItem = collectionItem;
            }
            //catch(Exception ex)
            //{
            //    throw new Exception();
            //}
            return databaseItem;
        }

        public List<CollectionItem> GetCollectionByUsername(string username)
        {
            List<CollectionItem> collection = new List<CollectionItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT card.id, card.name, card.img, card.price, card.tcg_url, collection_card.quantity, collection_card.amount_to_trade FROM card" +
                        " JOIN collection_card ON card.id = collection_card.id" +
                        " JOIN collection ON collection_card.collection_id = collection.collection_id" +
                        " JOIN users ON collection.user_id = users.user_id WHERE users.username = @username;", conn);

                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Card card = new Card();
                        card = cardDao.getCardFromReader(reader);
                        CollectionItem item = getCollectionItemFromReader(reader, card);
                        collection.Add(item);
                    }
                }
            }
            catch (SqlException)
            {
                throw new System.Exception();
            }
            return collection;
        }

        public CollectionItem getCollectionItemFromReader(SqlDataReader reader, Card card)
        {
            CollectionItem item = new CollectionItem();
            item.Card = card;
            item.Quantity = Convert.ToInt32(reader["quantity"]);
            item.QuantityForTrade = Convert.ToInt32(reader["amount_to_trade"]);
            return item;
        }

    }

    
}
