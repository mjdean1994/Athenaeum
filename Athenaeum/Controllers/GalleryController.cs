using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.Utils;
using Athenaeum.ViewModels.Gallery;
using Athenaeum.ViewModels.News;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;

namespace Athenaeum.Controllers
{
    public class GalleryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Gallery/
        public ActionResult Index(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var numOfPictures = db.Pictures.Count();

            var pictures =
                db.Pictures.OrderByDescending(x => x.CreatedDate).Skip(16 * (pageNumber.Value - 1)).Take(16).ToList();

            while (pictures.Count <= 0 && numOfPictures > 0)
            {
                pageNumber--;
                pictures =
                    db.Pictures.OrderByDescending(x => x.CreatedDate).Skip(16 * (pageNumber.Value - 1)).Take(16).ToList();
            }

            var vm = new GalleryIndexViewModel
            {
                Pictures = pictures,
                CurrentPage = pageNumber.Value,
                LastPage = (int)Math.Ceiling(numOfPictures / 10.0)
            };

            return View(vm);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var vPicture = db.Pictures.Find(id);

            if (vPicture == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(vPicture);
        }

        public ActionResult DetailsName(string name)
        {
            var picture = db.Pictures.FirstOrDefault(x => x.Title == name);

            if (picture == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Details", new {id = picture.PictureId});
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
        public ActionResult PostCreate(string title, string type, string description, HttpPostedFileBase image)
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
                errorList.Add("You must input a title for your picture.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Create", new { errorString });
            }

            var imageUrl = ImageUtilities.Upload(title + "_by_" + User.Identity.GetUserName(), image, 800, 450);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return RedirectToAction("Create");
            }

            var nPicture = new Picture
            {
                CreatedDate = DateTime.Now,
                Description = description.Replace("\n", "[br]"),
                ImageUrl = imageUrl,
                OwnerId = User.Identity.GetUserId(),
                Title = title,
                Type = type,
                UpdatedDate = DateTime.Now
            };

            db.Pictures.Add(nPicture);

            db.SaveChanges();

            return RedirectToAction("Details", new {id = nPicture.PictureId});
        }

        public ActionResult Edit(int id, string errorString)
        {
            var vPicture = db.Pictures.Find(id);

            if (vPicture == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (vPicture.Owner.UserName == User.Identity.GetUserName() || User.IsInRole("admin"))
            {
                ViewBag.Error = errorString;
                return View(vPicture);
            }

            return RedirectToAction("Details", new {id});
        }

        [HttpPost]
        public ActionResult PostEdit(int pictureId, string title, string type, string description)
        {
            var ePicture = db.Pictures.Find(pictureId);

            if (
                !(ePicture.Owner.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new {id = pictureId});
            }

            var isValid = true;
            var errorList = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                isValid = false;
                errorList.Add("You must input a title for your picture.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Edit", new { id = pictureId, errorString });
            }

            ePicture.Title = title;
            ePicture.Type = type;
            ePicture.Description = description.Replace("\n", "[br]");

            db.SaveChanges();

            return RedirectToAction("Details", new { id = ePicture.PictureId });
        }

        public ActionResult Delete(int id)
        {
            var dPicture = db.Pictures.Find(id);

            if (
                !(dPicture.Owner.UserName == User.Identity.GetUserName() || User.IsInRole("admin")))
            {
                return RedirectToAction("Details", new {id});
            }

            db.Pictures.Remove(dPicture);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 5 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
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
                CommentTypeId = 5,
                EntityId = id,
                PostedDate = DateTime.Now
            };

            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new { result = true });
        }
	}
}