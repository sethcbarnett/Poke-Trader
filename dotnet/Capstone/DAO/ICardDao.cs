using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public interface ICardDao
    {
        Card GetCardById(string Id);

        void AddCardToDatabase(Card card);

        void AddAllCardsToDatabase(List<Card> cards);

        Card getCardFromReader(SqlDataReader reader);
    }
}
