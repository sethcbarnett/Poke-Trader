using System.Collections.Generic;

namespace Capstone.Models
{
    public class Trade
    {
        public int TradeId { get; set; }
        public int UserIdTo { get; set; }
        public int UserIdFrom { get; set; }
        public string Status { get; set; }

        public List<CollectionItem> CollectionItemsFrom { get; set; }
        public List<CollectionItem> CollectionItemsTo { get; set; }

        public Trade()
        {

        }
        public Trade(int TradeId, int UserIdTo, int UserIdFrom, string Status, List<CollectionItem> CollectionItemsFrom, List<CollectionItem> CollectionItemsTo)
        { 
            this.TradeId = TradeId;
            this.UserIdTo = UserIdTo;   
            this.UserIdFrom = UserIdFrom;
            this.Status = Status;
            this.CollectionItemsFrom = CollectionItemsFrom;
            this.CollectionItemsTo = CollectionItemsTo;
        
        }



    }
}
