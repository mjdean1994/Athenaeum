using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Athenaeum.Models
{
    public class ContactMessage
    {
        public int ContactMessageId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? AnsweredDate { get; set; }
        public string AnsweredById { get; set; }
        
        [ForeignKey("AnsweredById")]
        public virtual ApplicationUser AnsweredBy { get; set; }
    }
}