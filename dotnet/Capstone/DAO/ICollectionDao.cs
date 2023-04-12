using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public interface ICollectionDao
    {
        List<CollectionItem> GetCollectionByUsername(string username);

        CollectionItem getCollectionItemFromReader(SqlDataReader reader, Card card);

    }
}
