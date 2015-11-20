using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Shared
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string PostedDate { get; set; }
        public DateTime PostedDateRaw { get; set; }
    }
}