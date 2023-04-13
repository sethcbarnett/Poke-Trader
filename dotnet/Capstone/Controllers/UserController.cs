using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDao userDao;
        

        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet("public")]
        public IActionResult GetPublicUsers()
        {
            List<PublicCollectionUser> users = userDao.GetPublicUsers();
            return Ok(users);
        }
        
        [HttpPut("{userName}")]
        public IActionResult ChangeToPremium(string userName) 
        {
           userDao.ChangeUsersToPremium(userName);
           return Ok();
        }
        [HttpPut("toggle/{userName}")]
        public IActionResult ToggleVisibility(string userName)
        {
            userDao.ToggleCollectionPrivacy(userName);
            return Ok();
        }
    }
}
