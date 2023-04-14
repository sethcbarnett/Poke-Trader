using Capstone.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;

namespace Capstone.Services
{
    public class CardApiService : ICardApiService
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
                newCard.Rarity = (string)card["rarity"];

                Dictionary<string, Object> images = (Dictionary<string, Object>)card["images"];
                newCard.Img = (string)images["small"];

                try
                {
                    Dictionary<string, Object> tcgplayeritems = (Dictionary<string, Object>)card["tcgplayer"];
                    newCard.TcgUrl = (string)tcgplayeritems["url"];
                }
                catch (Exception)
                {
                    newCard.TcgUrl = "";
                }
                try
                {
                    Dictionary<string, Object> tcgplayeritems = (Dictionary<string, Object>)card["tcgplayer"];
                    Dictionary<string, Object> priceInfo = (Dictionary<string, Object>)((Dictionary<string, Object>)tcgplayeritems["prices"]).ElementAt(0).Value;
                    newCard.Price = Convert.ToString(priceInfo["market"]);
                    newCard.LowPrice = Convert.ToString(priceInfo["low"]);
                    newCard.HighPrice = Convert.ToString(priceInfo["high"]);
                }
                catch (Exception) {
                    newCard.TcgUrl = "";
                    newCard.Price = "Price Not Found";
                    newCard.LowPrice = "Price Not Found";
                    newCard.HighPrice = "Price Not Found";
                }
                cards.Add(newCard);
            }
            return cards;
        }
    }
}
