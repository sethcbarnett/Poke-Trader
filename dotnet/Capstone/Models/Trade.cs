namespace Capstone.Models
{
    public class Trade
    {
        public int TradeId { get; set; }
        public int UserIdTo { get; set; }
        public int UserIdFrom { get; set; }
        public string Status { get; set; }

        public Trade()
        {

        }
        public Trade(int TradeId, int UserIdTo, int UserIdFrom, string Status)
        { 
            this.TradeId = TradeId;
            this.UserIdTo = UserIdTo;   
            this.UserIdFrom = UserIdFrom;
            this.Status = Status;
        
        }



    }
}
