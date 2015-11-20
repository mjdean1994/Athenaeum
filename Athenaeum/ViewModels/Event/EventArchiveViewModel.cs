using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Event
{
    public class EventArchiveViewModel
    {
        public List<Athenaeum.Models.Event> Events { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
    }
}