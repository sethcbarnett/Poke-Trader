using Microsoft.AspNetCore.Connections;
using System.Buffers.Text;
using System.Data;
using System.Numerics;
using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography.Xml;

namespace Capstone.DAO
{
    public class TradeSQLDao
    {
        private readonly string connectionString;

        public void TradeSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Trade GetTrade(int tradeId)
        {
            return null;
        }

        public Trade AttemptTrade(Trade trade)
        {

            return trade;
        }
        public void UpdateTrade(Trade trade)
        {

        }

        public void DeleteTrade(int tradeId)
        {

        }
        public List<Trade> GetTradeList(int userId)
        {
            return null;
        }

        public void AcceptTrade(Trade trade)
        {
        }
        
        public void RejectTrade(Trade trade)
        {
        }

        public Trade FulfillTrade(Trade trade)
        {
            return null;
        }

        private Trade CreateTradeFromReader(SqlDataReader reader)
        {
            Trade trade = new Trade();
            trade.TradeId = Convert.ToInt32(reader["trade_id"]);
            trade.UserIdTo = Convert.ToInt32(reader["user_id_to"]);
            trade.UserIdFrom = Convert.ToInt32(reader["user_id_from"]);
            trade.Status = Convert.ToString(reader["status"]);
            return trade;
        }

    }
}
