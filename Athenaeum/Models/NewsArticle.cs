using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class NewsArticle
    {
        public int NewsArticleId { get; protected set; }
        public string Title { get; set; }
        public string AuthorId { get; set; } 
        public DateTime PostedDate { get; set; }
        public string Body { get; set; }
        public int NewsCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfComments { get; set; }

        [ForeignKey("NewsCategoryId")]
        public virtual NewsCategory Category { get; set; }

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
    }
}