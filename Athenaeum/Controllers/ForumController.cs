using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.ViewModels.Forum;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Athenaeum.Controllers
{
    public class ForumController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Forum
        public ActionResult Index()
        {
            return View(db.ForumCategories.OrderBy(x => x.SortOrder).ToList());
        }

        public ActionResult Section(int id)
        {
            return View(db.ForumSections.Find(id));
        }

        public ActionResult Thread(int id)
        {
            var vm = new ThreadViewModel
            {
                Posts = db.ForumPosts.Where(x => x.ForumThreadId == id).OrderBy(x => x.PostedDate).ToList(),
                Thread = db.ForumThreads.Find(id)
            };

            vm.Section = db.ForumSections.First(x => x.ForumSectionId == vm.Thread.ForumSectionId);

            return View(vm);
        }

        public ActionResult CreateThread(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult CreatePost(int threadId, string body)
        {
            var nPost = new ForumPost
            {
                Body = body.Replace("\n", "[br]"),
                ForumThreadId = threadId,
                OwnerId = User.Identity.GetUserId(),
                PostedDate = DateTime.Now,
                IsEdited = false
            };

            db.ForumPosts.Add(nPost);
            db.SaveChanges();

            var eThread = db.ForumThreads.Find(nPost.ForumThreadId);
            eThread.UpdatedDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Thread", new {id = threadId});
        }

        [HttpPost]
        public ActionResult PostThread(string title, string body, int sectionId)
        {
            var nThread = new ForumThread()
            {
                Title = title,
                ForumSectionId = sectionId,
                IsLocked = false,
                IsSticky = false,
                UpdatedDate = DateTime.Now
            };

            db.ForumThreads.Add(nThread);
            db.SaveChanges();

            var nPost = new ForumPost
            {
                Body = body.Replace("\n", "[br]"),
                ForumThreadId = nThread.ForumThreadId,
                OwnerId = User.Identity.GetUserId(),
                PostedDate = DateTime.Now
            };

            db.ForumPosts.Add(nPost);
            db.SaveChanges();

            return RedirectToAction("Thread", new {id = nThread.ForumThreadId});
        }

        public ActionResult EditPost(int id)
        {
            var ePost = db.ForumPosts.Find(id);

            if (ePost == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(ePost);
        }

        public ActionResult SoftDelete(int id)
        {
            var ePost = db.ForumPosts.Find(id);
            ePost.Body = "[i]This post has been deleted.[/i]";
            ePost.IsLocked = true;

            db.SaveChanges();

            return RedirectToAction("Thread", new {id = ePost.ForumThreadId});
        }

        public ActionResult HardDelete(int id)
        {
            var dPost = db.ForumPosts.Find(id);
            db.ForumPosts.Remove(dPost);
            db.SaveChanges();

            return RedirectToAction("Thread", new { id = dPost.ForumThreadId });
        }

        [HttpPost]
        public ActionResult GrabPreview(int id, string body)
        {
            var pPost = db.ForumPosts.Find(id);
            pPost.Body = body;
            return PartialView("_Preview", pPost);
        }

        [HttpPost]
        public ActionResult GrabNewPreview(string body)
        {
            var nPost = new ForumPost
            {
                Body = body,
                PostedDate = DateTime.Now,
                Owner = db.Users.Find(User.Identity.GetUserId()),
                IsEdited = false
            };

            return PartialView("_Preview", nPost);
        }

        [HttpPost]
        public ActionResult PostEditPost(int id, string body)
        {
            var ePost = db.ForumPosts.Find(id);

            ePost.Body = body.Replace("\n", "[br]");
            ePost.IsEdited = true;

            db.SaveChanges();

            return RedirectToAction("Thread", new {id = ePost.ForumThreadId});
        }

        [HttpPost]
        public ActionResult CreateCategory(string category, int sortOrder)
        {
            var nCategory = new ForumCategory
            {
                Name = category,
                SortOrder = sortOrder
            };

            db.ForumCategories.Add(nCategory);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateSection(string section, int categoryId, string description)
        {
            var nSection = new ForumSection
            {
                Name = section,
                Description = description,
                ForumCategoryId = categoryId
            };

            db.ForumSections.Add(nSection);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LockToggle(int id)
        {
            var eThread = db.ForumThreads.Find(id);

            eThread.IsLocked = !eThread.IsLocked;

            db.SaveChanges();

            return RedirectToAction("Thread", new {id});
        }

        public ActionResult StickyToggle(int id)
        {
            var eThread = db.ForumThreads.Find(id);

            eThread.IsSticky = !eThread.IsSticky;

            db.SaveChanges();

            return RedirectToAction("Thread", new {id});
        }
    }
}