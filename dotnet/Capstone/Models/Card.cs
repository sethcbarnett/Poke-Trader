namespace Capstone.Models
{
    public class Card
    {
        public string id { get; set; }
        public string name { get; set; }

        public Card(string id, string name) 
        {
            this.id = id;
            this.name = name;
        }
    }
}
