using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.WarZone
{
    public class WarZoneDetailsViewModel
    {
        public int WarZoneId { get; set; }
        public string Name { get; set; }
        public string AllianceForce { get; set; }
        public string HordeForce { get; set; }
        public float AlliancePercent { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Influence { get; set; }
        public int Limit { get; set; }
        public string Continent { get; set; }
        public List<Battle> Battles { get; set; }
    }
}