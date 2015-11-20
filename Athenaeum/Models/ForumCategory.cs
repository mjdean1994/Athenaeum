using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class ForumCategory
    {
        public ForumCategory()
        {
            Sections = new List<ForumSection>();
        }

        public int ForumCategoryId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public virtual ICollection<ForumSection> Sections { get; set; } 
    }
}