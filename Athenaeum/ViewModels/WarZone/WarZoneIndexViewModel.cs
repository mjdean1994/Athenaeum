using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.WarZone
{
    public class WarZoneIndexViewModel
    {
        public int WarZoneId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name { get; set; }
        public float AlliancePercent { get; set; }
        public string Continent { get; set; }
    }
}