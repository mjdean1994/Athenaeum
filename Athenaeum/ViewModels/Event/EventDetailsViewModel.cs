using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.Event
{
    public class EventDetailsViewModel
    {
        public Athenaeum.Models.Event Event { get; set; }
        public int CurrentUserStatus { get; set; }
    }
}