namespace Capstone.Models
{
    public class Card
    {
        public string id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }

        public Card(string id, string name, string imgUrl) 
        {
            this.id = id;
            this.name = name;
            this.imgUrl = imgUrl;
        }

        public Card()
        {
        }
    }
}
