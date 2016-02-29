using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Athenaeum.Models
{
    public class Rsvp
    {
        public int RsvpId { get; set; }

        public string UserId { get; set; }
        public int EventId { get; set; }
        //0 - Tentative, 1 - Accepted
        public int Status { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }
}