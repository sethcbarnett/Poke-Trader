using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICollectionDao
    {
        List<Card> GetCollectionByUser(int userId);

    }
}
