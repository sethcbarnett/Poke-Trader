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
    public class TradeSqlDao : ITradeDao
    {
        private readonly string connectionString;

        public TradeSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public string AddTrade(Trade trade)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //See if there is already a trade between the active users

                SqlCommand isTradeAlreadyActive = new SqlCommand("SELECT * FROM trade WHERE (user_id_from = (SELECT user_id FROM users WHERE username = @userOne) " +
                                                                 "AND user_id_to = (SELECT user_id FROM users WHERE username = @userTwo)) OR " +
                                                                 "(user_id_from = (SELECT user_id FROM users WHERE username = @userTwo) " +
                                                                 "AND user_id_to = (SELECT user_id FROM users WHERE username = @userOne)) ", conn);
                isTradeAlreadyActive.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                isTradeAlreadyActive.Parameters.AddWithValue("@userTwo", trade.UsernameTo);
                SqlDataReader hasOpenTrade = isTradeAlreadyActive.ExecuteReader();
                if (hasOpenTrade.Read())
                {
                    return "These users already have an open trade.";
                }

                //Add trade to trade table
                SqlCommand addToTradeTable = new SqlCommand("INSERT INTO trade (user_id_from, user_id_to) " +
                                                            "OUTPUT INSERTED.trade_id " +
                                                            "VALUES " +
                                                            "((SELECT user_id FROM users WHERE username = @userOne), " +
                                                            "(SELECT user_id FROM users WHERE username = @userTwo));", conn);
                addToTradeTable.Parameters.AddWithValue("@userOne", trade.UsernameFrom);
                addToTradeTable.Parameters.AddWithValue("@userTwo", trade.UsernameTo);

                int tradeId = Convert.ToInt32(addToTradeTable.ExecuteNonQuery());

                //Add collection Items From to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsFrom)
                {
                    SqlCommand addCollectionItemsFrom = new SqlCommand("INSERT INTO trade_card_collection (trade_id, id, collection_id) VALUES (@tradeId," +
                                                                       "(SELECT id FROM card where name = @cardName)," +
                                                                       "(SELECT collection_id FROM collection WHERE user_id = " +
                                                                       "(SELECT user_id from users WHERE username = @username)));", conn);
                    addCollectionItemsFrom.Parameters.AddWithValue("@tradeId", tradeId);
                    addCollectionItemsFrom.Parameters.AddWithValue("@cardName", item.Card.Name);
                    addCollectionItemsFrom.Parameters.AddWithValue("@username", trade.UsernameFrom);

                    addCollectionItemsFrom.ExecuteNonQuery();
                }

                //Add collection Items To to the trade_card_collection join table
                foreach (CollectionItem item in trade.CollectionItemsTo)
                {
                    SqlCommand addCollectionItemsFrom = new SqlCommand("INSERT INTO trade_card_collection (trade_id, id, collection_id) VALUES (@tradeId," +
                                                                       "(SELECT id FROM card where name = @cardName)," +
                                                                       "(SELECT collection_id FROM collection WHERE user_id = " +
                                                                       "(SELECT user_id from users WHERE username = @username)));", conn);
                    addCollectionItemsFrom.Parameters.AddWithValue("@tradeId", tradeId);
                    addCollectionItemsFrom.Parameters.AddWithValue("@cardName", item.Card.Name);
                    addCollectionItemsFrom.Parameters.AddWithValue("@username", trade.UsernameTo);

                    addCollectionItemsFrom.ExecuteNonQuery();
                }
            }
            return "Hooray";
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

        //private Trade CreateTradeFromReader(SqlDataReader reader)
        //{
        //    Trade trade = new Trade();
        //    trade.TradeId = Convert.ToInt32(reader["trade_id"]);
        //    trade.UserIdTo = Convert.ToInt32(reader["user_id_to"]);
        //    trade.UserIdFrom = Convert.ToInt32(reader["user_id_from"]);
        //    trade.Status = Convert.ToString(reader["status"]);
        //    return trade;
        //}

    }
}
