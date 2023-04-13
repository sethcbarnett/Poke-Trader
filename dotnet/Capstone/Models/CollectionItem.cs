namespace Capstone.Models
{
    public class CollectionItem
    {
        public Card Card { get; set; }
        public int Quantity { get; set; }
        public int QuantityForTrade { get; set; }
        public int CollectionId { get; set; }
        public CollectionItem() { }
        public CollectionItem(Card card, int quantity, int quantityForTrade, int   collectionId)
        {
            Card = card;
            Quantity = quantity;
            QuantityForTrade = quantityForTrade;
            CollectionId = collectionId;
        }
    }
}
