using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("trade")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeDao tradeDao;
        public TradeController(ITradeDao tradeDao)
        {
            this.tradeDao = tradeDao;
        }

        [HttpPost("{usernameFrom}/{usernameTo}")]
        public IActionResult AddTrade(string usernameFrom, string usernameTo, Trade trade)
        {
            string result = tradeDao.AddTrade(trade);
            if (result == "Hooray")
            {
                return Ok();
            }
            return BadRequest(result);
        }

        [HttpGet("{usernameFrom}/{usernameTo}")]
        public IActionResult GetTrade(string usernameFrom, string usernameTo)
        {
            Trade trade = tradeDao.GetTrade(usernameFrom, usernameTo);
            if (trade.TradeId == 0)
            {
                return NotFound();
            }
            return Ok(trade);
        }
    }
}
