using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Athenaeum.Models;

namespace Athenaeum.ViewModels.Gallery
{
    public class GalleryIndexViewModel
    {
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}