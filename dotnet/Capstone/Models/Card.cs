namespace Capstone.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Price { get; set; }
        public string TcgUrl { get; set; }

        public Card(string id, string name, string img, string price, string tcgUrl) 
        {
            this.Id = id;
            this.Name = name;
            this.Img = img;
            this.Price = price;
            this.TcgUrl= tcgUrl;
        }

        public Card() { }
    }
}
