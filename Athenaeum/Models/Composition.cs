using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class Composition
    {
        public int CompositionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Type { get; set; } //hardcoded
        public bool IsFeatured { get; set; }
        public string AuthorId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
    }
}