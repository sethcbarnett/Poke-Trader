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
                try
                {
                    Card newCard = new Card();

                    newCard.Id = (string)card["id"];
                    newCard.Name = (string)card["name"];
                    newCard.Rarity = (string)card["rarity"];
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
                catch(Exception e)
                {
                    throw new Exception();
                }
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
