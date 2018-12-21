using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapitalSoftWebSite.Models;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    //[Authorize]
    public class TechnologiesController : Controller
    {
        public  async Task<ActionResult> Index()
        {
            IList<Technology> list = await DbAdaptor.GetTechnologiesAsync();
            return View(list);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Technology technology = await DbAdaptor.GetTechnologyAsync(id);
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
        public async Task<ActionResult> Create(Technology technology)
        {
            if (ModelState.IsValid)
            {
                await DbAdaptor.CreateTechnologyAsync(technology);
                return RedirectToAction("Index");
            }

            return View(technology);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Technology technology = await DbAdaptor.GetTechnologyAsync(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Technology technology)
        {
            if (ModelState.IsValid)
            {
                await DbAdaptor.EditTechnologyAsync(technology);
                return RedirectToAction("Index");
            }
            return View(technology);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Technology technology = await DbAdaptor.GetTechnologyAsync(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await DbAdaptor.DeleteTechnologyAsync(id);
            return RedirectToAction("Index");
        }
    }
}
