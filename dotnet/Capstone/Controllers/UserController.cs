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
        [HttpGet("{search}")]
        public IActionResult GetUsernamesBySearch(string searchString)
        {
            List<string> searchedUsers = userDao.GetUsernamesBySearch(searchString);
            if(searchedUsers.Count == 0)
            {
                return NotFound();
            }
            return Ok(searchedUsers);
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
