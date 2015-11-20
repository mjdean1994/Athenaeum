using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class ForumThread
    {
        public ForumThread()
        {
            Posts = new List<ForumPost>();
        }

        public int ForumThreadId { get; set; }
        public string Title { get; set; }
        public int ForumSectionId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsSticky { get; set; }
        public bool IsLocked { get; set; }

        [ForeignKey("ForumSectionId")]
        public virtual ForumSection Section { get; set; }

        public virtual ICollection<ForumPost> Posts { get; set; }
    }
}