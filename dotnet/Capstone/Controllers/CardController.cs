using Capstone.DAO;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardDao cardDao;

        public CardController(ICardDao _cardDao)
        {
            cardDao = _cardDao;
        }

        [HttpGet("{id}")]
        public IActionResult GetCardById(string id)
        {
            Card card = cardDao.GetCardById(id);
            return Ok(card);
        }
    }
}
