using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionDao collectionDao;

        public CollectionController(ICollectionDao _collectionDao)
        {
            collectionDao = _collectionDao;
        }
        [HttpGet("{userId}")]
        public IActionResult GetCardById(int userId)
        {
            List<Card> cards = collectionDao.GetCollectionByUser(userId);
            return Ok(cards);
        }

    }
}
