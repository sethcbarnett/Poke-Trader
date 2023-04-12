using Capstone.DAO;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    [Route("card")]
    [ApiController]
    public class OwnedCardController : Controller
    {
        private readonly ICardDao cardDao;

        public OwnedCardController(ICardDao _cardDao)
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
