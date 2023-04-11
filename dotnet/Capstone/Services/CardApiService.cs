using Capstone.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Capstone.Services
{
    public class CardApiService
    {
        public string apiUrl = @"https://api.pokemontcg.io/v2/cards";
        public IRestClient client; 

        public CardApiService() 
        {
            client = new RestClient(apiUrl);
        }

        public List<Card> GetAllCards()
        {
            RestRequest request = new RestRequest();
            IRestResponse<Dictionary<string, Object>> response = client.Get<Dictionary<string,Object>>(request);
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

                Dictionary<string, Object> images = (Dictionary<string, Object>)card["images"];
                newCard.ImgUrl = (string)images["small"];
                cards.Add(newCard);
            }
            return cards;
        }
    }
}
