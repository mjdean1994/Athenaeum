using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class Battle
    {
        public int BattleId { get; set; }
        public string Name { get; set; }
        public string AllianceCommander { get; set; }
        public string HordeCommander { get; set; }
        public int AllianceForces { get; set; }
        public int HordeForces { get; set; }
        public string Description { get; set; }
        public string Outcome { get; set; }
        public DateTime Date { get; set; }
        public int WarZoneId { get; set; }
        public int NewInfluence { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ReporterId { get; set; }

        [ForeignKey("WarZoneId")]
        public virtual WarZone WarZone { get; set; }
    }
}