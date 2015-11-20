using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Character
{
    public class CharacterEditViewModel
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string Guild { get; set; }
        public string Introduction { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string History { get; set; }
        public string Status { get; set; }
    }
}