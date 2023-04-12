using Capstone.Models;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("search")]
    [ApiController]
    public class CardSearchController
    {
        private readonly ICardApiService cardApiService;
        public CardSearchController(ICardApiService cardApiService)
        {
            this.cardApiService = cardApiService;
        }

        [HttpGet("params/{parameters}")]
        public ActionResult<List<Card>> searchByParameters(string parameters)
        {
            try
            {
                cardApiService.AddSearchParameters(parameters);
                List<Card> cards = cardApiService.GetCardsByParameters();
                if (cards.Count > 0) return new OkObjectResult(cards);
                else return new NotFoundObjectResult(0);
            }
            catch(Exception)
            {
                return new NotFoundObjectResult(0);
            }
        }
    }
}
