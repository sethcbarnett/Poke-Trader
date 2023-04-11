namespace Capstone.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public Card(string id, string name, string imgUrl) 
        {
            this.Id = id;
            this.Name = name;
            this.ImgUrl = imgUrl;
        }

        public Card()
        {
        }
    }
}
