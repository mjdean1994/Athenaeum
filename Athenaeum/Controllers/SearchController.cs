using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.ViewModels.Search;

namespace Athenaeum.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult Submit(string query)
        {
            return RedirectToAction("Index", new {query});
        }

        public ActionResult Index(string query)
        {
            var vm = new SearchResultViewModel();
            vm.Characters = db.Characters.Where(x => x.Name.Contains(query) || x.FullName.Contains(query)).OrderBy(x => x.Name).ToList();
            vm.Guilds = db.Guilds.Where(x => x.Name.Contains(query)).OrderBy(x => x.Name).ToList();
            vm.Compositions = db.Compositions.Where(x => x.Title.Contains(query)).OrderBy(x => x.Title).ToList();
            vm.Pictures = db.Pictures.Where(x => x.Title.Contains(query)).OrderBy(x => x.Title).ToList();
            vm.Events = db.Events.Where(x => x.Title.Contains(query)).OrderByDescending(x => x.StartDate).ToList();
            vm.Threads = db.ForumThreads.Where(x => x.Title.Contains(query)).OrderByDescending(x => x.UpdatedDate).ToList();


            return View(vm);
        }
	}
}