using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class NewsCategory
    {
        public int NewsCategoryId { get; protected set; }
        public string Description { get; set; }
    }
}