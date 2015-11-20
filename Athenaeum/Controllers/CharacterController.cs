using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Athenaeum.Models;
using Athenaeum.Services;
using Athenaeum.Utils;
using Athenaeum.ViewModels.Character;
using Athenaeum.ViewModels.Shared;
using Microsoft.AspNet.Identity;
using WowDotNetAPI;

namespace Athenaeum.Controllers
{
    public class CharacterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Character/
        public ActionResult Index(string startLetter)
        {
            if (string.IsNullOrEmpty(startLetter))
            {
                startLetter = "A";
            }

            var vm = db.Characters.Where(x => x.Name.StartsWith(startLetter)).OrderBy(x => x.Name).Select(x => new CharacterIndexViewModel
            {
                CharacterId = x.CharacterId,
                Class = x.Class,
                FullName = x.FullName,
                Name = x.Name,
                Race = x.Race
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var character = db.Characters.Find(id);

            if (character == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var uri =
                string.Format(
                    "/character/wyrmrest-accord/{0}?fields=pvp",
                    character.Name);

            var vm = new CharacterDetailsViewModel
            {
                Appearance = character.Appearance,
                CharacterId = character.CharacterId,
                Class = character.Class,
                FullName = character.FullName,
                Guild = character.Guild,
                History = character.History,
                ImageUrl = character.ImageUrl,
                Introduction = character.Introduction,
                Name = character.Name,
                Personality = character.Personality,
                Race = character.Race,
                Owner = character.Owner.UserName,
                Status = character.Status
            };

            return View(vm);
        }

        public ActionResult DetailsName(string name)
        {
            var character = db.Characters.FirstOrDefault(x => x.Name == name);

            if (character == null)
            {
                return RedirectToAction("Error", new {name = name});
            }

            return RedirectToAction("Details", new {id = character.CharacterId});
        }

        public ActionResult Error(string name)
        {
            ViewBag.Character = name;

            return View();
        }

        [HttpGet]
        public JsonResult LoadComments(int id)
        {
            var data =
                db.Comments.Where(x => x.CommentTypeId == 2 && x.EntityId == id).OrderByDescending(x => x.PostedDate)
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
                CommentTypeId = 2,
                EntityId = id,
                PostedDate = DateTime.Now
            };
            
            db.Comments.Add(nComment);
            db.SaveChanges();

            return Json(new { result = true });
        }

        public ActionResult Create(string errorString)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = errorString;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult PostCreate(string name, string fullname, string race, string charClass, string guild,
            string introduction, string personality, string appearance, string history, HttpPostedFileBase image)
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
                errorList.Add("You must input a name for your character.");
            }
            else if (name.Length < 2)
            {
                isValid = false;
                errorList.Add("Your character name must be two or more letters.");
            }
            else if (name.Length > 12)
            {
                isValid = false;
                errorList.Add("Your character name cannot exceed twelve letters.");
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

            var imageUrl = ImageUtilities.Upload(name + "_portrait_" + DateTime.Now.ToString("MM_dd_yyyy"), image, 800, 600);

            var nCharacter = new Character
            {
                Name = name,
                FullName = fullname,
                Race = race,
                Class = charClass,
                Guild = guild,
                Introduction = introduction.Replace("\n", "[br]"),
                Personality = personality.Replace("\n", "[br]"),
                Appearance = appearance.Replace("\n", "[br]"),
                History = history,
                ImageUrl = imageUrl,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                Status = "Active"
            };

            if (string.IsNullOrEmpty(imageUrl))
            {
                switch (nCharacter.Race)
                {
                    case "Blood Elf":
                        nCharacter.ImageUrl = "http://i.imgur.com/u3zcyl3.png";
                        break;
                    case "Draenei":
                        nCharacter.ImageUrl = "http://i.imgur.com/e5yK2Nw.png";
                        break;
                    case "Dwarf":
                        nCharacter.ImageUrl = "http://i.imgur.com/w9MEJh5.png";
                        break;
                    case "Gnome":
                        nCharacter.ImageUrl = "http://i.imgur.com/CQC1Mls.png";
                        break;
                    case "Goblin":
                        nCharacter.ImageUrl = "http://i.imgur.com/GP3262t.png";
                        break;
                    case "Human":
                        nCharacter.ImageUrl = "http://i.imgur.com/63yLEiZ.png";
                        break;
                    case "Night Elf":
                        nCharacter.ImageUrl = "http://i.imgur.com/c40VGsU.png";
                        break;
                    case "Orc":
                        nCharacter.ImageUrl = "http://i.imgur.com/lsypB13.png";
                        break;
                    case "Tushui Pandaren":
                    case "Huojin Pandaren":
                        nCharacter.ImageUrl = "http://i.imgur.com/qkvMsP7.png";
                        break;
                    case "Tauren":
                        nCharacter.ImageUrl = "http://i.imgur.com/sz4YfVx.png";
                        break;
                    case "Troll":
                        nCharacter.ImageUrl = "http://i.imgur.com/yF8Sx0H.png";
                        break;
                    case "Forsaken":
                        nCharacter.ImageUrl = "http://i.imgur.com/aaCSy4R.png";
                        break;
                    case "Worgen":
                        nCharacter.ImageUrl = "http://i.imgur.com/9gf14Au.png";
                        break;
                    default:
                        nCharacter.ImageUrl =
                            "http://www.sparkdesignprofessionals.org/sites/default/files/styles/event_small/public/default_images/profile_no_img.gif?itok=_4GAiNuA";
                        break;
                }
            }

            db.Characters.Add(nCharacter);
            db.SaveChanges();

            return RedirectToAction("Details", "Character", new {id = nCharacter.CharacterId});
        }

        public ActionResult Edit(int id, string errorString)
        {
            var eCharacter = db.Characters.Find(id);

            if (eCharacter == null)
            {
                return RedirectToAction("Index");
            }

            if (User.Identity.GetUserName() == eCharacter.Owner.UserName || User.IsInRole("admin"))
            {
                var vm = new CharacterEditViewModel
                {
                    CharacterId = eCharacter.CharacterId,
                    Appearance = eCharacter.Appearance,
                    Class = eCharacter.Class,
                    FullName = eCharacter.FullName,
                    Guild = eCharacter.Guild,
                    History = eCharacter.History,
                    Introduction = eCharacter.Introduction,
                    Name = eCharacter.Name,
                    Personality = eCharacter.Personality,
                    Status = eCharacter.Status
                };

                ViewBag.Error = errorString;
                return View(vm);
            }

            return RedirectToAction("Details", "Character", new {id});
        }

        [HttpPost]
        public ActionResult PostEdit(int id, string name, string fullname, string charClass, string status, string guild,
            string introduction, string personality, string appearance, string history, HttpPostedFileBase image)
        {
            var eCharacter = db.Characters.Find(id);

            if (User.Identity.GetUserName() == eCharacter.Owner.UserName || User.IsInRole("admin"))
            {
                var isValid = true;
                var errorList = new List<string>();

                if (string.IsNullOrEmpty(name))
                {
                    isValid = false;
                    errorList.Add("You must input a name for your character.");
                }
                else if (name.Length < 2)
                {
                    isValid = false;
                    errorList.Add("Your character name must be two or more letters.");
                }
                else if (name.Length > 12)
                {
                    isValid = false;
                    errorList.Add("Your character name cannot exceed twelve letters.");
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

                var imageUrl = ImageUtilities.Upload(name + "_portrait", image, 800, 600);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    eCharacter.ImageUrl = imageUrl;
                }

                eCharacter.Name = name;
                eCharacter.FullName = fullname;
                eCharacter.Class = charClass;
                eCharacter.Guild = guild;
                eCharacter.Status = status;
                eCharacter.Introduction = introduction.Replace("\n", "[br]");
                eCharacter.Personality = personality.Replace("\n", "[br]");
                eCharacter.Appearance = appearance.Replace("\n", "[br]");
                eCharacter.History = history.Replace("\n", "[br]");
                eCharacter.UpdatedDate = DateTime.Now;

                db.SaveChanges();
            }

            return RedirectToAction("Details", "Character", new { id = eCharacter.CharacterId });
        }

        public ActionResult Delete(int id)
        {
            var dCharacter = db.Characters.Find(id);

            if (User.Identity.GetUserName() == dCharacter.Owner.UserName || User.IsInRole("admin"))
            {
                db.Characters.Remove(dCharacter);

                db.SaveChanges();

                return RedirectToAction("Index", "Character");
            }

            return RedirectToAction("Details", "Character", new {id = dCharacter.CharacterId});
        }
	}
}