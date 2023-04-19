
using Capstone.Models;
using System.Collections.Generic;




namespace Capstone.DAO
{
    public interface ITradeDao
    {
        void AddTrade(Trade trade);

        Trade GetTrade(string userOne, string userTwo);

        public void RejectTrade(string userOne, string userTwo);

        public void AcceptTrade(string userOne, string userTwo);

    }

}
