using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class CardCollection
    {
       public Boolean IsPublic { get; set; }

        public CardCollection() { }

        public CardCollection(Boolean isPublic) 
        { 
        
            IsPublic = isPublic;
        }
    }
}
