using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class Comment
    {
        public int CommentId { get; protected set; }
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime PostedDate { get; set; }

        public int EntityId { get; set; }
        public int CommentTypeId { get; set; }

        [ForeignKey("CommentTypeId")]
        public virtual CommentType Type { get; set; }

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
    }
}