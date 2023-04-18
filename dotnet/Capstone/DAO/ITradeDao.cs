
using Capstone.Models;
using System.Collections.Generic;




namespace Capstone.DAO
{
    public interface ITradeDao
    {
        string AddTrade(Trade trade);

        Trade GetTrade(string userOne, string userTwo);

    }

}
