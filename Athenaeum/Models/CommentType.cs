using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.Models
{
    public class CommentType
    {
        public int CommentTypeId { get; protected set; }
        public string Description { get; set; }
        /* This is a mess but it's the only good way to do it
         *      1 - news
         * 
         * 
         */
    }
}