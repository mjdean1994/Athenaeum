using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Athenaeum.Controllers
{
    public class ResourcesController : Controller
    {
        // GET: Resources
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditorGuide()
        {
            return View();
        }

        public ActionResult Timeline()
        {
            return View();
        }
    }
}