using System.Collections.Generic;

namespace Capstone.Models
{
    public class CardCollection
    {
        public List<CollectionItem> cards { get; set; }

        public CardCollection() { }

        public CardCollection(List<CollectionItem> cards) { this.cards = cards; }
    }
}
