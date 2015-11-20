using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.ViewModels.Composition;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;

namespace Athenaeum.Controllers
{
    public class CompositionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Composition
        public ActionResult Index(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var numOfComps = db.Compositions.Count();

            var comps =
                db.Compositions.OrderByDescending(x => x.UpdatedDate).Skip(20 * (pageNumber.Value - 1)).Take(20).ToList();

            while (comps.Count <= 0 && numOfComps > 0)
            {
                pageNumber--;
                comps =
                db.Compositions.OrderByDescending(x => x.UpdatedDate).Skip(20 * (pageNumber.Value - 1)).Take(20).ToList();
            }

            var vm = new CompositionIndexViewModel
            {
                Compositions = comps,
                CurrentPage = pageNumber.Value,
                LastPage = (int)Math.Ceiling(numOfComps / 10.0)
            };

            return View(vm);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var details = db.Compositions.Find(id);

            if (details == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(details);
        }

        public ActionResult DetailsName(string name)
        {
            var composition = db.Compositions.FirstOrDefault(x => x.Title == name);

            if (composition == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Details", new { id = composition.CompositionId });
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
        public ActionResult PostCreate(string title, string type, string body)
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
                errorList.Add("You must input a title for your composition.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Create", new {errorString });
            }

            var nComposition = new Composition
            {
                AuthorId = User.Identity.GetUserId(),
                Body = body.Replace("\n", "[br]"),
                CreatedDate = DateTime.Now,
                IsFeatured = false,
                Title = title,
                Type = type,
                UpdatedDate = DateTime.Now
            };

            db.Compositions.Add(nComposition);
            db.SaveChanges();

            return RedirectToAction("Details", new {id = nComposition.CompositionId});
        }

        public ActionResult Edit(int? id, string errorString)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var eComposition = db.Compositions.Find(id);

            if (eComposition == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (eComposition.Author.UserName == User.Identity.GetUserName() || User.IsInRole("admin") ||
                User.IsInRole("god"))
            {
                ViewBag.Error = errorString;
                return View(eComposition);
            }

            return RedirectToAction("Details", new {id});
        }

        [HttpPost]
        public ActionResult PostEdit(int compositionId, string title, string type, string body)
        {
            var eComposition = db.Compositions.Find(compositionId);

            if (!(eComposition.Author.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new {id = compositionId});
            }

            var isValid = true;
            var errorList = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                isValid = false;
                errorList.Add("You must input a title for your composition.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Edit", new { id = compositionId, errorString });
            }

            eComposition.Title = title;
            eComposition.Type = type;
            eComposition.Body = body.Replace("\n", "[br]");
            eComposition.UpdatedDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = eComposition.CompositionId });
        }

        public ActionResult FeatureToggle(int? compositionId)
        {
            if (compositionId == null)
            {
                return RedirectToAction("Index");
            }

            if (!(User.IsInRole("admin") || User.IsInRole("god")))
            {
                return RedirectToAction("Details", new {id = compositionId});
            }

            var eComposition = db.Compositions.Find(compositionId);

            eComposition.IsFeatured = !eComposition.IsFeatured;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = eComposition.CompositionId });
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 4 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
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

        [HttpPost]
        public JsonResult AddComment(string body, int id)
        {
            var nComment = new Comment
            {
                Body = body,
                AuthorId = User.Identity.GetUserId(),
                CommentTypeId = 4,
                EntityId = id,
                PostedDate = DateTime.Now
            };

            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new { result = true });
        }

        public ActionResult Delete(int id)
        {
            var dComposition = db.Compositions.Find(id);

            if (User.Identity.GetUserName() == dComposition.Author.UserName || User.IsInRole("admin"))
            {
                db.Compositions.Remove(dComposition);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}