using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Athenaeum.ViewModels.Composition
{
    public class CompositionIndexViewModel
    {
        public List<Athenaeum.Models.Composition> Compositions { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }

    }
}