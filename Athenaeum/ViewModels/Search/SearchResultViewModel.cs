using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Search
{
    public class SearchResultViewModel
    {
        public List<Athenaeum.Models.Character> Characters { get; set; } 
        public List<Athenaeum.Models.Guild> Guilds { get; set; } 
        public List<Athenaeum.Models.Composition> Compositions { get; set; } 
        public List<Athenaeum.Models.Picture> Pictures { get; set; } 
        public List<Athenaeum.Models.Event> Events { get; set; } 
        public List<Athenaeum.Models.ForumThread> Threads { get; set; } 
    }
}