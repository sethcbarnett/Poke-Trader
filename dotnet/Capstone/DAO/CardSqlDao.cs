using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class CardSqlDao : ICardDao
    {
        public string connectionString;
        public CardSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Card GetCardById(string Id)
        {
            Card card = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM card WHERE api_card_id = @id", conn);

                    cmd.Parameters.AddWithValue("@id", Id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        card = getCardFromReader(reader);
                    }
                }
            }
            catch(SqlException)
            {
                throw new System.Exception();
            }
            return card;
        }

        public Card getCardFromReader(SqlDataReader reader)
        {
            Card card = new Card()
            {
                Id = Convert.ToString(reader["id"]),
                Name = Convert.ToString(reader["name"]),
                Img = Convert.ToString(reader["img"]),
                Price = Convert.ToString(reader["price"]),
                LowPrice = Convert.ToString(reader["low_price"]),
                HighPrice = Convert.ToString(reader["high_price"]),
                Rarity = Convert.ToString(reader["rarity"]),
                TcgUrl = Convert.ToString(reader["tcg_url"])
            };
            return card;
        }

        public void AddCardToDatabase(Card card)
        {

        }

        public void AddAllCardsToDatabase(List<Card> cards)
        {

        }
    }
}
