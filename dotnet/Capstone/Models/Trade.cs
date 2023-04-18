using System.Collections.Generic;

namespace Capstone.Models
{
    public class Trade
    {
        public int TradeId { get; set; }
        public string UsernameTo { get; set; }
        public string UsernameFrom { get; set; }
        public string Status { get; set; }

        public List<CollectionItem> CollectionItemsFrom { get; set; }
        public List<CollectionItem> CollectionItemsTo { get; set; }

        public Trade()
        {

        }
        public Trade(int TradeId, string UsernameFrom, string UsernameTo, string Status, List<CollectionItem> CollectionItemsFrom, List<CollectionItem> CollectionItemsTo)
        { 
            this.TradeId = TradeId;
            this.UsernameTo = UsernameTo;   
            this.UsernameFrom = UsernameFrom;
            this.Status = Status;
            this.CollectionItemsFrom = CollectionItemsFrom;
            this.CollectionItemsTo = CollectionItemsTo;
        
        }



    }
}
