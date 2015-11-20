using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.ViewModels;
using Athenaeum.ViewModels.Home;

namespace Athenaeum.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel();
            vm.Articles = db.NewsArticles.OrderByDescending(x => x.PostedDate).Take(5).ToList();
            vm.Threads = db.ForumThreads.OrderByDescending(x => x.UpdatedDate).Take(3).ToList();
            vm.Pictures = db.Pictures.OrderByDescending(x => x.CreatedDate).Take(9).ToList();
            vm.Events =
                db.Events.Where(x => x.EndDate > DateTime.Now).OrderByDescending(x => x.StartDate).Take(3).ToList();

            return View(vm);
        }

        public ActionResult Explore()
        {
            var vm = new HomeExploreViewModel();
            vm.Characters = db.Characters.OrderByDescending(x => x.UpdatedDate).Take(5).ToList();
            vm.Guilds = db.Guilds.OrderByDescending(x => x.UpdatedDate).Take(5).ToList();
            vm.Compositions = db.Compositions.OrderByDescending(x => x.UpdatedDate).Take(5).ToList();
            vm.Pictures = db.Pictures.OrderByDescending(x => x.CreatedDate).Take(4).ToList();

            return View(vm);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendContact(string name, string email, string message)
        {
            var nContactMessage = new ContactMessage
            {
                Name = name,
                Body = message,
                Email = email,
                CreatedDate = DateTime.Now
            };

            db.ContactMessages.Add(nContactMessage);
            db.SaveChanges();

            return RedirectToAction("Contact", "Home");
        }

        public ActionResult DeleteComment(int id)
        {
            var dComment = db.Comments.Find(id);

            var typeId = dComment.CommentTypeId;
            var entityId = dComment.EntityId;

            db.Comments.Remove(dComment);
            db.SaveChanges();

            switch (typeId)
            {
                case 2:
                    return RedirectToAction("Details", "Character", new {id = entityId});
                    break;
                case 3:
                    return RedirectToAction("Details", "Guild", new { id = entityId });
                    break;
                case 4:
                    return RedirectToAction("Details", "Composition", new { id = entityId });
                    break;
                case 5:
                    return RedirectToAction("Details", "Gallery", new { id = entityId });
                    break;
                case 1:
                    return RedirectToAction("Details", "News", new { id = entityId });
                    break;
                case 6:
                    return RedirectToAction("Details", "Event", new { id = entityId });
                    break;
                default:
                    return RedirectToAction("Index");
            }

        }
    }
}