using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public interface ICollectionDao
    {
        List<CollectionItem> GetCollectionByUsername(string username);

        CollectionItem getCollectionItemFromReader(SqlDataReader reader, Card card);

        CollectionItem AddCollectionItemToCollection(CollectionItem collectionItem, string username);

        void UpdateCard(string username, CollectionItem item);

    }
}
