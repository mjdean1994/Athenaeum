using System.Collections.Generic;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<NewsArticle> Articles { get; set; }
        public List<ForumThread> Threads { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<Models.Event> Events { get; set; } 
    }
}