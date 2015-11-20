using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class Guild
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Tagline { get; set; }
        public string Introduction { get; set; }
        public string Background { get; set; }
        public string OocInformation { get; set; }
        public string OwnerId { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}