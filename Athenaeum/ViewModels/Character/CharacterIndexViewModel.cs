﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Character
{
    public class CharacterIndexViewModel
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
    }
}