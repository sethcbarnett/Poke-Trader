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

        public void GetAllCards()
        {
            RestRequest request = new RestRequest();
            IRestResponse<Root> response = client.Get<Root>(request);

            // TODO: catch exceptions

            Console.WriteLine("hello");

            
        }
    }
}
