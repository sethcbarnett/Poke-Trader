using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
