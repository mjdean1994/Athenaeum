using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class ForumSection
    {
        public ForumSection()
        {
            Threads = new List<ForumThread>();
        }

        public int ForumSectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ForumCategoryId { get; set; }

        [ForeignKey("ForumCategoryId")]
        public virtual ForumCategory Category { get; set; }

        public virtual ICollection<ForumThread> Threads { get; set; }
    }
}