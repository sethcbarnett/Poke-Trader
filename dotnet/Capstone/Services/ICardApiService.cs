using Capstone.Models;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Capstone.Services
{
    public interface ICardApiService
    {
        public void AddNameSearchParameter(string name);
        public void AddTypeSearchParameter(string type);
        public List<Card> GetCardsByParameters();
        public List<Card> DeserializeJsonResponse(IRestResponse<Dictionary<string, Object>> response);
        public void AddSearchParameters(string searchParameters);
        public void ResetSearchParameters();
    }
}
