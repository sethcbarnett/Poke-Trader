namespace Capstone.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Price { get; set; }
        public string LowPrice { get; set; }
        public string HighPrice { get; set; }
        public string Rarity { get; set; }
        public string TcgUrl { get; set; }

        public Card(string id, string name, string img, string price, string lowPrice, string highPrice, string rarity, string tcgUrl) 
        {
            this.Id = id;
            this.Name = name;
            this.Img = img;
            this.Price = price;
            this.LowPrice = lowPrice;
            this.HighPrice = highPrice;
            this.Rarity = rarity;
            this.TcgUrl= tcgUrl;
        }

        public Card() { }
    }
}
