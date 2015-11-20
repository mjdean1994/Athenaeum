using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class ForumPost
    {
        public int ForumPostId { get; set; }
        public string Body { get; set; }
        public string OwnerId { get; set; }
        public int ForumThreadId { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsEdited { get; set; }
        public bool IsLocked { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        [ForeignKey("ForumThreadId")]
        public virtual ForumThread Thread { get; set; }

    }
}