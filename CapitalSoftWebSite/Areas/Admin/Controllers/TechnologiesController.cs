using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapitalSoftWebSite.Models;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    //[Authorize]
    public class TechnologiesController : Controller
    {
        public ActionResult Index()
        {
            return View(new DbAdaptor().GetTechnologies());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Technology technology = new DbAdaptor().GetTechnology(id);
            if (technology == null)
                return HttpNotFound();
            return View(technology);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Technology technology)
        {
            if (ModelState.IsValid)
            {
                new DbAdaptor().CreateTechnology(technology);
                return RedirectToAction("Index");
            }

            return View(technology);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Technology technology = new DbAdaptor().GetTechnology(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Technology technology)
        {
            if (ModelState.IsValid)
            {
                new DbAdaptor().EditTechnology(technology);
                return RedirectToAction("Index");
            }
            return View(technology);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Technology technology = new DbAdaptor().GetTechnology(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new DbAdaptor().DeleteTechnology(id);
            return RedirectToAction("Index");
        }
    }
}
