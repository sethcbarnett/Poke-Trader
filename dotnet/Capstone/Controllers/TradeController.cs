using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpPost]
        public IActionResult AddTrade(Trade trade)
        {
            try
            {
                tradeDao.AddTrade(trade);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{usernameFrom}/{usernameTo}")]
        public IActionResult GetTrade(string usernameFrom, string usernameTo)
        {
            try
            {
                Trade trade = tradeDao.GetTrade(usernameFrom, usernameTo);
                return Ok(trade);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{usernameFrom}/{usernameTo}/{status}")]
        public IActionResult UpdateTrade(string usernameFrom, string usernameTo, string status)
        {
            if (status == "rejected")
            {
                try
                {
                    tradeDao.RejectTrade(usernameFrom, usernameTo);
                    return Ok();
                }
                catch(Exception e)
                { return BadRequest(e.Message); }
            }
            else if (status == "accepted")
            {
                try
                {
                    tradeDao.AcceptTrade(usernameFrom, usernameTo);
                    return Ok();
                }
                catch (Exception e)
                { return BadRequest(e.Message); }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
