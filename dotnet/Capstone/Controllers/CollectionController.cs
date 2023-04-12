﻿using Capstone.DAO;
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
        [HttpGet("{username}")]
        public IActionResult GetCollectionByUsername(string username)
        {
            List<CollectionItem> cards = collectionDao.GetCollectionByUsername(username);
            return Ok(cards);
        }

        [HttpPost("{username}")]
        public IActionResult AddCardToCollection(string username, CollectionItem item)
        {
            CollectionItem returnedItem = collectionDao.AddCollectionItemToCollection(item, username);
            if (returnedItem != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
