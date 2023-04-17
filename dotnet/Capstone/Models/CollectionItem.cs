namespace Capstone.Models
{
    public class CollectionItem
    {
        public Card Card { get; set; }
        public int Quantity { get; set; }
        public int QuantityForTrade { get; set; }
        public string Grade { get; set; }
        public CollectionItem() { }
        public CollectionItem(Card card, int quantity, int quantityForTrade, string grade)
        {
            Card = card;
            Quantity = quantity;
            QuantityForTrade = quantityForTrade;
            Grade = grade;  
        }
    }
}
