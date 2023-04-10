using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICardDao
    {
        Card GetCardById(int id);

        void AddCardToDatabase(Card card);

        void AddAllCardsToDatabase(List<Card> cards);
    }
}
