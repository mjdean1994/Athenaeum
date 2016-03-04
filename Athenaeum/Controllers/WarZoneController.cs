using Athenaeum.Models;
using Athenaeum.ViewModels.WarZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Athenaeum.Controllers
{
    public class WarZoneController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WarZone
        public ActionResult Index()
        {
            var vm = db.WarZones.Select(x => new WarZoneIndexViewModel
            {
                WarZoneId = x.WarZoneId,
                Name = x.Name,
                UpdatedDate = x.UpdatedDate,
                Continent = x.Continent,
                AlliancePercent = x.Influence >= 0 ? ((x.Influence / x.Limit) * 100) + 50 : 50 - ((x.Influence / x.Limit) * 100)
            }).OrderBy(x => x.Name).ToList();

            return View(vm);
        }
        
        public ActionResult Create()
        {
            if(!User.IsInRole("admin") && !User.IsInRole("god"))
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult PostCreate(string name, string allianceForce, string hordeForce, string continent)
        {
            if (!User.IsInRole("admin") && !User.IsInRole("god"))
            {
                return RedirectToAction("Index");
            }

            var nWarZone = new WarZone
            {
                Name = name,
                AllianceForce = allianceForce,
                HordeForce = hordeForce,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Continent = continent,
                Influence = 0,
                Limit = 20
            };

            db.WarZones.Add(nWarZone);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = nWarZone.WarZoneId });
        }

        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var warzone = db.WarZones.Find(id);

            if (warzone == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var vm = new WarZoneDetailsViewModel
            {
                Name = warzone.Name,
                AllianceForce = warzone.AllianceForce,
                HordeForce = warzone.HordeForce,
                UpdatedDate = warzone.UpdatedDate,
                AlliancePercent = warzone.Influence >= 0 ? ((warzone.Influence / warzone.Limit) * 100) + 50 : 50 - ((warzone.Influence / warzone.Limit) * 100),
                Influence = warzone.Influence,
                Limit = warzone.Limit,
                Battles = warzone.Battles.ToList()
            };

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            var eWarZone = db.WarZones.Find(id);

            if(eWarZone == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var vm = new WarZoneDetailsViewModel
            {
                WarZoneId = eWarZone.WarZoneId,
                Name = eWarZone.Name,
                AllianceForce = eWarZone.AllianceForce,
                HordeForce = eWarZone.HordeForce,
                Continent = eWarZone.Continent
            };

            return View(vm);
        }

        public ActionResult PostEdit(int warZoneId, string name, string allianceForce, string hordeForce, string continent)
        {
            var eWarZone = db.WarZones.Find(warZoneId);

            eWarZone.Name = name;
            eWarZone.AllianceForce = allianceForce;
            eWarZone.HordeForce = hordeForce;
            eWarZone.Continent = continent;
            eWarZone.UpdatedDate = DateTime.Now;
            
            db.SaveChanges();

            return RedirectToAction("Details", new { id = warZoneId });
        }
    }
}