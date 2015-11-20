using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.Utils;
using Athenaeum.ViewModels.News;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.Win32;

namespace Athenaeum.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /News/
        public ActionResult Archive(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var numOfArticles = db.NewsArticles.Count();

            var articles =
                db.NewsArticles.OrderByDescending(x => x.PostedDate).Skip(10 * (pageNumber.Value - 1)).Take(10).ToList();

            while (articles.Count <= 0 && numOfArticles > 0)
            {
                pageNumber--;
                articles =
                    db.NewsArticles.OrderByDescending(x => x.PostedDate).Skip(10 * (pageNumber.Value - 1)).Take(10).ToList();
            }

            var vm = new NewsArchiveViewModel
            {
                Articles = articles,
                CurrentPage = pageNumber.Value,
                LastPage = (int) Math.Ceiling(numOfArticles/10.0),
                Categories = db.NewsCategories.OrderBy(x => x.Description).ToList()
            };

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var article = db.NewsArticles.Find(id);
            if (article == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(article);
        }

        [HttpPost]
        public ActionResult Create(string title, int category, string body, HttpPostedFileBase image)
        {
            var imageUrl = ImageUtilities.Upload(title + "_header", image, 900, 600);

            var nNewsArticle = new NewsArticle
            {
                AuthorId = User.Identity.GetUserId(),
                Title = title,
                PostedDate = DateTime.Now,
                NewsCategoryId = category,
                ImageUrl = imageUrl,
                NumberOfComments = 0,
                Body = body.Replace("\n", "[br]")
            };

            db.NewsArticles.Add(nNewsArticle);
            db.SaveChanges();

            return RedirectToAction("Archive");
        }

        [HttpPost]
        public ActionResult CreateCategory(string category)
        {
            var nCategory = new NewsCategory
            {
                Description = category
            };

            db.NewsCategories.Add(nCategory);
            db.SaveChanges();

            return RedirectToAction("Archive");
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 1 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
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
                CommentTypeId = 1,
                EntityId = id,
                PostedDate = DateTime.Now
            };

            var article = db.NewsArticles.Find(id);
            article.NumberOfComments++;

            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new {result = true});
        }

        [HttpPost]
        public ActionResult Edit(int id, string body)
        {
            var eArticle = db.NewsArticles.Find(id);

            eArticle.Body = body.Replace("\n", "[br]");

            db.SaveChanges();

            return RedirectToAction("Details", new {id});
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dArticle = db.NewsArticles.Find(id);
            db.NewsArticles.Remove(dArticle);
            db.SaveChanges();

            return RedirectToAction("Archive", "News");
        }
	}
}