using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.Utils;
using Athenaeum.ViewModels.Character;
using Athenaeum.ViewModels.Guild;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;

namespace Athenaeum.Controllers
{
    public class GuildController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Guild/
        public ActionResult Index(string startLetter)
        {
            var vm = db.Guilds.Where(x => x.Name.StartsWith(startLetter)).OrderBy(x => x.Name).Select(x => new GuildIndexViewModel
            {
                Faction = x.Faction,
                GuildId = x.GuildId,
                Name = x.Name
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var guild = db.Guilds.Find(id);

            var vm = new GuildDetailsViewModel
            {
                Background = guild.Background,
                Faction = guild.Faction,
                GuildId = guild.GuildId,
                ImageUrl = guild.ImageUrl,
                Introduction = guild.Introduction,
                Name = guild.Name,
                OocInformation = guild.OocInformation,
                Owner = guild.Owner.UserName,
                Tagline = guild.Tagline,
                Status = guild.Status
            };

            return View(vm);
        }

        public ActionResult DetailsName(string name)
        {
            var guild = db.Guilds.FirstOrDefault(x => x.Name == name);

            if (guild == null)
            {
                return RedirectToAction("Error", "Guild", new {name});
            }

            return RedirectToAction("Details", "Guild", new {id = guild.GuildId});
        }

        public ActionResult Error(string name)
        {
            ViewBag.Guild = name;

            return View();
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
        public ActionResult PostCreate(string name, string tagline, string faction, string introduction,
            string background, string ooc, HttpPostedFileBase image)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var isValid = true;
            var errorList = new List<string>();

            if (string.IsNullOrEmpty(name))
            {
                isValid = false;
                errorList.Add("You must input a name for your guild.");
            }
            else if (name.Length < 2)
            {
                isValid = false;
                errorList.Add("Your guild name must be two or more letters.");
            }
            else if (name.Length > 24)
            {
                isValid = false;
                errorList.Add("Your guild name cannot exceed 24 letters.");
            }

            if (introduction.Length > 1000)
            {
                isValid = false;
                errorList.Add("Introduction cannot exceed 1000 characters.");
            }
            if (!isValid)
            {
                var errorString = string.Empty;
                errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                return RedirectToAction("Create", new { errorString });
            }

            var imageUrl = ImageUtilities.Upload(name + "_banner", image, 1600, 320);

            var nGuild = new Guild
            {
                Name = name,
                Tagline = tagline,
                Faction = faction,
                Introduction = introduction.Replace("\n", "[br]"),
                Background = background.Replace("\n", "[br]"),
                OocInformation = ooc.Replace("\n", "[br]"),
                ImageUrl = imageUrl,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                Status = "Active"
            };

            db.Guilds.Add(nGuild);
            db.SaveChanges();

            return RedirectToAction("Details", "Guild", new { id = nGuild.GuildId });
        }

        public ActionResult Edit(int id, string errorString)
        {
            var eGuild = db.Guilds.Find(id);

            if (eGuild.Owner.UserName != User.Identity.GetUserName() && !User.IsInRole("admin"))
            {
                return RedirectToAction("Details", new {id});
            }

            var vm = new GuildEditViewModel
            {
                Background = eGuild.Background,
                Faction = eGuild.Faction,
                GuildId = eGuild.GuildId,
                Introduction = eGuild.Introduction,
                Name = eGuild.Name,
                OocInformation = eGuild.OocInformation,
                Tagline = eGuild.Tagline,
                Status = eGuild.Status
            };

            ViewBag.Error = errorString;
            return View(vm);
        }

        [HttpPost]
        public ActionResult PostEdit(int id, string name, string tagline, string status, string introduction, string background,
            string ooc, HttpPostedFileBase image)
        {
            var eGuild = db.Guilds.Find(id);



            if (User.Identity.GetUserName() == eGuild.Owner.UserName || User.IsInRole("admin"))
            {
                var isValid = true;
                var errorList = new List<string>();

                if (string.IsNullOrEmpty(name))
                {
                    isValid = false;
                    errorList.Add("You must input a name for your guild.");
                }
                else if (name.Length < 2)
                {
                    isValid = false;
                    errorList.Add("Your guild name must be two or more letters.");
                }
                else if (name.Length > 24)
                {
                    isValid = false;
                    errorList.Add("Your guild name cannot exceed 24 letters.");
                }

                if (introduction.Length > 1000)
                {
                    isValid = false;
                    errorList.Add("Introduction cannot exceed 1000 characters.");
                }
                if (!isValid)
                {
                    var errorString = string.Empty;
                    errorString = errorList.Aggregate(errorString, (current, item) => current + ("[br]" + item));
                    return RedirectToAction("Edit", new { id, errorString });
                }

                var imageUrl = ImageUtilities.Upload(name + "_portrait_" + DateTime.Now.ToString("MM_dd_yyyy"), image, 1600, 320);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    eGuild.ImageUrl = imageUrl;
                }

                eGuild.Name = name;
                eGuild.Tagline = tagline;
                eGuild.Introduction = introduction.Replace("\n", "[br]");
                eGuild.Background = background.Replace("\n", "[br]");
                eGuild.OocInformation = ooc.Replace("\n", "[br]");
                eGuild.Status = status;

                db.SaveChanges();
            }

            return RedirectToAction("Details", "Guild", new { id = eGuild.GuildId });
        }

        public ActionResult Delete(int id)
        {
            var dGuild = db.Guilds.Find(id);

            if (User.Identity.GetUserName() == dGuild.Owner.UserName || User.IsInRole("admin"))
            {
                db.Guilds.Remove(dGuild);
                db.SaveChanges();

                return RedirectToAction("Index", "Guild");
            }

            return RedirectToAction("Details", new {id});
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 3 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
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
                CommentTypeId = 3,
                EntityId = id,
                PostedDate = DateTime.Now
            };

            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new { result = true });
        }

        [HttpGet]
        public JsonResult LoadMembers(string name)
        {
            var characters =
                db.Characters.Where(x => x.Guild == name).OrderBy(x => x.Name).Select(sel => new CharacterIndexViewModel
                {
                    CharacterId = sel.CharacterId,
                    Class = sel.Class,
                    FullName = sel.FullName,
                    Name = sel.Name,
                    Race = sel.Race
                }).ToList();

            return Json(characters, JsonRequestBehavior.AllowGet);
        }
	}
}