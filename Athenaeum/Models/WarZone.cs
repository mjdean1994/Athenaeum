using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class WarZone
    {
        public WarZone()
        {
            Battles = new List<Battle>();
        }

        public int WarZoneId { get; set; }
        public string Name { get; set; }
        public string AllianceForce { get; set; }
        public string HordeForce { get; set; }
        public int Influence { get; set; }
        public int Limit { get; set; }
        public string Continent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<Battle> Battles { get; set; }

    }
}