using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Controllers
{
    public class PortfolioController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IList<Project> list = await DbAdaptor.GetProjectsFullAsync(cultureName);
            return View(list);
        }


        [HttpGet]
        public async Task<ActionResult> More(int id)
        {
            ViewBag.Portfolio = true;

            Project model = await DbAdaptor.GetProjectAsync(id);
            if (model == null)
                return HttpNotFound();
            return PartialView("~/Views/Home/_More.cshtml", model);

           // return RedirectToAction("More", "Home", new { id = id });
        }
    }
}
