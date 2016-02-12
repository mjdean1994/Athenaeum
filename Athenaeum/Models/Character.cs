using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Athenaeum.Models
{
    public class Character
    {
        public Character()
        {
        }

        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public string Guild { get; set; }
        public string Introduction { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string History { get; set; }
        public string ImageUrl { get; set; }
        public string OwnerId { get; set; }
        public string Status { get; set; }
        public int? ArmoryCharacterId { get; set; }

        public DateTime? LastPull { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}