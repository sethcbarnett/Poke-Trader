using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryWritingScript
{
    internal class Program
    {
        CardApiService service;
        static void Main(string[] args)
        {
            //Get all pickachus
            //Program.WritePikachuLoverQueries();

            //Get Ash's pokemon
            //Program.WriteKetchumQueries();

            //Get Team rocket's pokemon
            Program.WriteRocketQueries();
        }

        public static string MakeInsertIntoCardQuery(Card card)
        {
            string query = $"INSERT INTO card (name, id, img, price, low_price, high_price, rarity, tcg_url) VALUES " +
                           $"('{card.Name}', '{card.Id}', '{card.Img}', '{card.Price}', '{card.LowPrice}', '{card.HighPrice}', '{card.Rarity}', '{card.TcgUrl}');";
            return query;
        }

        public static string MakeInsertIntoCollectionCardQuery(int collectionId, string id)
        {
            string query = $"INSERT INTO collection_card (collection_id, id, quantity, amount_to_trade) VALUES " +
                           $"({collectionId}, '{id}', 1, 1);";
            return query;
        }

        public static void WritePikachuLoverQueries()
        {
            CardApiService service = new CardApiService();
            service.AddSearchParameters("name:pikachu");
            List<Card> pikachus = service.GetCardsByParameters();

            string pikachuQueries = "";
            for (int i = 0; i < 101; i++)
            {
                pikachuQueries += Program.MakeInsertIntoCardQuery(pikachus[i]) + " \n";
            }
            Console.WriteLine(pikachuQueries);

            string pikachuLoverQueries = "";
            for (int i = 0; i < 101; i++)
            {
                pikachuLoverQueries += Program.MakeInsertIntoCollectionCardQuery(3, pikachus[i].Id) + " \n";
            }
            Console.WriteLine(pikachuLoverQueries);
        }

        public static void WriteKetchumQueries()
        {
            List<Card> AshCollection = new List<Card>();
            CardApiService service = new CardApiService();

            service.AddSearchParameters("name:\"Ash's Pikachu\"");
            List<Card> cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Pidgeot");
            cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Bulbasaur");
            cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Charizard");
            cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[5]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Snorlax");
            cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Dragonite");
            cardResults = service.GetCardsByParameters();
            AshCollection.Add(cardResults[0]);
            service.ResetSearchParameters();

            string AshQueries = "";
            foreach (Card card in AshCollection)
            {
                AshQueries += Program.MakeInsertIntoCardQuery(card) + " \n";
            }
            Console.WriteLine(AshQueries);

            string AshCollectionQueries = "";
            foreach (Card card in AshCollection)
            {
                AshCollectionQueries += Program.MakeInsertIntoCollectionCardQuery(4, card.Id) + " \n";
            }
            Console.WriteLine(AshCollectionQueries);
        }

        public static void WriteRocketQueries()
        {
            List<Card> RocketCollection = new List<Card>();
            CardApiService service = new CardApiService();

            service.AddSearchParameters("name:Meowth");
            List<Card> cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[4]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Wobbuffet");
            cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Seviper");
            cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[2]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Yanmega");
            cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[5]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Woobat");
            cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[0]);
            service.ResetSearchParameters();
            service.AddSearchParameters("name:Ekans");
            cardResults = service.GetCardsByParameters();
            RocketCollection.Add(cardResults[0]);
            service.ResetSearchParameters();

            string RocketQueries = "";
            foreach (Card card in RocketCollection)
            {
                RocketQueries += Program.MakeInsertIntoCardQuery(card) + " \n";
            }
            Console.WriteLine(RocketQueries);

            string RocketCollectionQueries = "";
            foreach (Card card in RocketCollection)
            {
                RocketCollectionQueries += Program.MakeInsertIntoCollectionCardQuery(4, card.Id) + " \n";
            }
            Console.WriteLine(RocketCollectionQueries);
        }
    }

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
            this.TcgUrl = tcgUrl;
        }

        public Card() { }
    }

    public class CardApiService
    {
        public string apiUrl = @"https://api.pokemontcg.io/v2/cards";
        public IRestClient client;
        public string searchParameters;

        public CardApiService()
        {
            client = new RestClient(apiUrl);
            searchParameters = "?q=";
        }

        public void AddNameSearchParameter(string name)
        {
            searchParameters += $"name:{name} ";
        }

        public void AddTypeSearchParameter(string type)
        {
            searchParameters += $"types:{type} ";
        }

        public void AddSearchParameters(string searchParameters)
        {
            this.searchParameters += searchParameters;
        }

        public void ResetSearchParameters()
        {
            searchParameters = "?q=";
        }

        public List<Card> GetCardsByParameters()
        {
            RestRequest request = new RestRequest(searchParameters);
            IRestResponse<Dictionary<string, Object>> response = client.Get<Dictionary<string, Object>>(request);
            return DeserializeJsonResponse(response);
        }

        public List<Card> DeserializeJsonResponse(IRestResponse<Dictionary<string, Object>> response)
        {
            RestSharp.JsonArray listOfJsonCards = (RestSharp.JsonArray)response.Data["data"];
            List<Card> cards = new List<Card>();

            foreach (Dictionary<string, Object> card in listOfJsonCards)
            {
                
                Card newCard = new Card();

                newCard.Id = (string)card["id"];
                newCard.Name = (string)card["name"];
                if (card.ContainsKey("rarity"))
                {
                    newCard.Rarity = (string)card["rarity"];
                }
                else newCard.Rarity = "";
                Dictionary<string, Object> images = (Dictionary<string, Object>)card["images"];
                newCard.Img = (string)images["small"];


                if (card.ContainsKey("tcgplayer"))
                {
                    Dictionary<string, Object> tcgplayeritems = (Dictionary<string, Object>)card["tcgplayer"];
                    newCard.TcgUrl = (string)tcgplayeritems["url"];
                    List<string> prices = getTcgPrices(tcgplayeritems);
                    newCard.LowPrice = prices[0];
                    newCard.Price = prices[1];
                    newCard.HighPrice = prices[2];
                    if (newCard.Price == "Price Not Found")
                    {
                        if (card.ContainsKey("cardmarket"))
                        {
                            Dictionary<string, Object> cardMarketItems = (Dictionary<string, Object>)card["cardmarket"];
                            prices = getCardMarketPrices(cardMarketItems);
                            newCard.LowPrice = prices[0];
                            newCard.Price = prices[1];
                            newCard.HighPrice = prices[2];
                        }
                    }
                }
                else if (card.ContainsKey("cardmarket"))
                {
                    Dictionary<string, Object> cardMarketItems = (Dictionary<string, Object>)card["cardmarket"];
                    newCard.TcgUrl = (string)cardMarketItems["url"];
                    List<string> prices = getCardMarketPrices(cardMarketItems);
                    newCard.LowPrice = prices[0];
                    newCard.Price = prices[1];
                    newCard.HighPrice = prices[2];
                }
                else
                {
                    newCard.TcgUrl = "";
                    newCard.Price = "Price Not Found";
                    newCard.LowPrice = "Price Not Found";
                    newCard.HighPrice = "Price Not Found";
                }
                cards.Add(newCard);
            }
            return cards;
        }

        public List<string> getTcgPrices(Dictionary<string, Object> websiteInfo)
        {
            Dictionary<string, Object> priceInfo = ((Dictionary<string, Object>)websiteInfo["prices"]);
            if (priceInfo.Count <= 0)
            {
                return new List<string>() { "Price Not Found", "Price Not Found", "Price Not Found" };
            }
            Dictionary<string, Object> priceDict = (Dictionary<string, Object>)priceInfo.ElementAt(0).Value;
            string lowPrice = Convert.ToString(priceDict["low"]);
            string midPrice = Convert.ToString(priceDict["market"]);
            if (midPrice == "")
            {
                midPrice = Convert.ToString(priceDict["mid"]);
            }
            if (midPrice == "")
            {
                midPrice = "Price Not Found";
            }
            string highPrice = Convert.ToString(priceDict["high"]);

            return new List<string>() { lowPrice, midPrice, highPrice };
        }

        public List<string> getCardMarketPrices(Dictionary<string, Object> websiteInfo)
        {
            string lowPrice;
            string midPrice;
            string highPrice = "Price Not Found";
            Dictionary<string, Object> priceInfo = ((Dictionary<string, Object>)websiteInfo["prices"]);
            if ((double)priceInfo["lowPrice"] == 0.0)
            {
                lowPrice = "Price Not Found";
            }
            else
            {
                lowPrice = Convert.ToString(priceInfo["lowPrice"]);
            }
            if ((double)priceInfo["averageSellPrice"] == 0.0)
            {
                midPrice = "Price Not Found";
            }
            else
            {
                midPrice = Convert.ToString(priceInfo["averageSellPrice"]);
            }
            return new List<string>() { lowPrice, midPrice, highPrice };
        }
    }
}
