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

        public List<Card> GetCollectionByUsername(string username)
        {
            List<Card> cards = new List<Card>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT card.api_card_id, card.name, card.image_url FROM card" +
                        " JOIN collection_card ON card.api_card_id = collection_card.api_card_id" +
                        " JOIN collection ON collection_card.collection_id = collection.collection_id" +
                        " JOIN users ON collection.user_id = users.user_id WHERE users.username = @username;", conn);

                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Card card= new Card();
                        card = cardDao.getCardFromReader(reader);
                        cards.Add(card);
                    }
                }
            }
            catch (SqlException)
            {
                throw new System.Exception();
            }
            return cards;
        }


    }
    
}
