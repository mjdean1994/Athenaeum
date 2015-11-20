using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.ViewModels.Admin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Athenaeum.Controllers
{
    [Authorize(Roles = "admin, god")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            var vm = new AdminIndexViewModel
            {
                UnansweredContactMessages = db.ContactMessages.Count(x => x.AnsweredDate == null)
            };

            return View(vm);
        }

        public ActionResult Inbox()
        {
            return View(db.ContactMessages.OrderByDescending(x => x.AnsweredDate == null).ThenByDescending(x => x.CreatedDate).ToList());
        }

        public ActionResult Users()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!roleManager.RoleExists("user"))
            {
                var nRole = new IdentityRole("user");
                roleManager.Create(nRole);
            }
            if (!roleManager.RoleExists("admin"))
            {
                var nRole = new IdentityRole("admin");
                roleManager.Create(nRole);
            }
            if (!roleManager.RoleExists("moderator"))
            {
                var nRole = new IdentityRole("moderator");
                roleManager.Create(nRole);
            }
            if (!roleManager.RoleExists("journalist"))
            {
                var nRole = new IdentityRole("journalist");
                roleManager.Create(nRole);
            }

            var vm = new AdminUsersViewModel();

            vm.Users = db.Users.OrderBy(x => x.UserName).ToList();
            vm.Roles = db.Roles.ToList();

            return View(vm);
        }

        [HttpPost]
        public JsonResult SaveUser(string userId, string userTitle, string roleId)
        {
            var user = db.Users.Find(userId);
            if (!string.IsNullOrEmpty(userTitle))
            {
                user.Title = userTitle;
            }
            user.Roles.Clear();

            var roleToAdd = db.Roles.Find(roleId);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.AddToRole(userId, roleToAdd.Name);

            db.SaveChanges();

            return Json(new {success = true});
        }

        public ActionResult MarkAsAnswered(int id)
        {
            var eMessage = db.ContactMessages.Find(id);
            eMessage.AnsweredDate = DateTime.Now;
            eMessage.AnsweredById = User.Identity.GetUserId();
            db.SaveChanges();

            return RedirectToAction("Inbox");
        }
	}
}