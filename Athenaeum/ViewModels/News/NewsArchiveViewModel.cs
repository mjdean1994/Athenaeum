using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.News
{
    public class NewsArchiveViewModel
    {
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public List<NewsArticle> Articles { get; set; }
        public List<NewsCategory> Categories { get; set; } 
    }
}