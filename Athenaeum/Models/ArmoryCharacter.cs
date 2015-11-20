using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Athenaeum.Models
{
    public class ArmoryCharacter
    {
        public int ArmoryCharacterId { get; set; }
        public int AchievementPoints { get; set; }
        public string Class { get; set; }
        public string PrimarySpec { get; set; }
        public string SecondarySpec { get; set; }
        public int Rating2v2 { get; set; }
        public int Rating3v3 { get; set; }
        public int Rating5v5 { get; set; }
        public int RatingRbg { get; set; }
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }
    }
}