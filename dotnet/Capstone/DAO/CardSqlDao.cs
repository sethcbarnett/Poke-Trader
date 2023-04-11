using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public class CardSqlDao : ICardDao
    {
        public string connectionString;
        public CardSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Card GetCardById(int id)
        {
            return new Card("123", "Good", "");
        }

        public void AddCardToDatabase(Card card)
        {

        }

        public void AddAllCardsToDatabase(List<Card> cards)
        {

        }
    }
}
