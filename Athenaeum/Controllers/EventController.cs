using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Athenaeum.Models;
using Athenaeum.Utils;
using Athenaeum.ViewModels.Event;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;

namespace Athenaeum.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Event/
        public ActionResult Index()
        {
            var model = db.Events.Where(x => x.EndDate > DateTime.Now).OrderBy(x => x.StartDate).ToList();

            return View(model);
        }

        public ActionResult Archive(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var numOfEvents = db.Events.Where(x => x.EndDate <= DateTime.Now).Count();

            var events =
                db.Events.Where(x => x.EndDate <= DateTime.Now).OrderByDescending(x => x.StartDate).Skip(10 * (pageNumber.Value - 1)).Take(10).ToList();

            while (events.Count <= 0 && numOfEvents > 0)
            {
                pageNumber--;
                events =
                    db.Events.Where(x => x.EndDate <= DateTime.Now).OrderByDescending(x => x.StartDate).Skip(10 * (pageNumber.Value - 1)).Take(10).ToList();
            }

            var vm = new EventArchiveViewModel
            {
                Events = events,
                CurrentPage = pageNumber.Value,
                LastPage = (int)Math.Ceiling(numOfEvents / 10.0)
            };

            return View(vm);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var vEvent = db.Events.Find(id);

            if (vEvent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vEvent);
        }

        public ActionResult Edit(int id, string errorString)
        {
            var eEvent = db.Events.Find(id);

            if (eEvent == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (
                !(eEvent.Author.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new {id});
            }

            ViewBag.Error = errorString;
            return View(eEvent);
        }

        [HttpPost]
        public ActionResult PostEdit(int id, string title, string faction, string introduction, string description,
            HttpPostedFileBase image, DateTime start, DateTime end)
        {
            var imageUrl = ImageUtilities.Upload(title + "_banner_" + DateTime.Now.ToString("MM_dd_yyyy"), image, 1600, 320);

            var eEvent = db.Events.Find(id);

            if (
                !(eEvent.Author.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new { id });
            }

            var isValid = true;
            var errorList = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                isValid = false;
                errorList.Add("You must input a title for your event.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Edit", new { id, errorString });
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                eEvent.ImageUrl = imageUrl;
            }

            eEvent.Title = title;
            eEvent.Faction = faction;
            eEvent.Introduction = introduction;
            eEvent.Description = description.Replace("\n", "[br]");
            eEvent.StartDate = start;
            eEvent.EndDate = end;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = eEvent.EventId });
        }

        public ActionResult Create(string errorString)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Error = errorString;
            return View();
        }

        [HttpPost]
        public ActionResult PostCreate(string title, string faction, string introduction, string description,
            HttpPostedFileBase image, DateTime start, DateTime end)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var isValid = true;
            var errorList = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                isValid = false;
                errorList.Add("You must input a title for your event.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Create", new { errorString });
            }

            var imageUrl = ImageUtilities.Upload(title + "_banner_" + DateTime.Now.ToString("MM_dd_yyyy"), image, 1600, 320);

            var nEvent = new Event
            {
                AuthorId = User.Identity.GetUserId(),
                Description = description.Replace("\n", "[br]"),
                Title = title,
                Faction = faction,
                StartDate = start,
                EndDate = end,
                Introduction = introduction,
                ImageUrl = imageUrl
            };

            db.Events.Add(nEvent);
            db.SaveChanges();

            return RedirectToAction("Details", new {id = nEvent.EventId});
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 6 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
                    .Select(sel => new CommentViewModel
                    {
                        CommentId = sel.CommentId,
                        Author = sel.Author.UserName,
                        Body = sel.Body,
                        ImageUrl = sel.Author.ImageUrl,
                        PostedDateRaw = sel.PostedDate
                    }).ToList();

            foreach (var comment in data)
            {
                comment.PostedDate = comment.PostedDateRaw.ToString("hh:mm:ss tt | MMMM dd, yyyy");
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var dEvent = db.Events.Find(id);

            if (
                !(dEvent.Author.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new {id});
            }

            db.Events.Remove(dEvent);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AddComment(string body, int id)
        {
            var nComment = new Comment
            {
                Body = body,
                AuthorId = User.Identity.GetUserId(),
                CommentTypeId = 6,
                EntityId = id,
                PostedDate = DateTime.Now
            };

            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new { result = true });
        }
	}
}