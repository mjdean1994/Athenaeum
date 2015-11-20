using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.Home
{
    public class HomeExploreViewModel
    {
        public List<Athenaeum.Models.Character> Characters { get; set; }
        public List<Athenaeum.Models.Guild> Guilds { get; set; }
        public List<Athenaeum.Models.Composition> Compositions { get; set; }
        public List<Athenaeum.Models.Picture> Pictures { get; set; }
    }
}