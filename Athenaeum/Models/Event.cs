using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class Event
    {
        public Event()
        {
            Attendees = new List<Rsvp>();
        }

        public int EventId { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AuthorId { get; set; }
        public string Faction { get; set; }
        public string ImageUrl { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Rsvp> Attendees { get; set; }
    }
}