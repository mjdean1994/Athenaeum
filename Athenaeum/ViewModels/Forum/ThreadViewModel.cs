using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.Forum
{
    public class ThreadViewModel
    {
        public ForumThread Thread { get; set; }
        public ForumSection Section { get; set; }
        public List<ForumPost> Posts { get; set; } 
    }
}