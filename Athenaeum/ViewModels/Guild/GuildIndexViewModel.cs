﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Guild
{
    public class GuildIndexViewModel
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string ImageUrl { get; set; }
        public string Introduction { get; set; }
        public string Tagline { get; set; }
    }
}