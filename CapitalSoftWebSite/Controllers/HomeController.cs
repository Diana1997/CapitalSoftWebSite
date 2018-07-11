using CapitalSoftWebSite.Helpers;
using CapitalSoftWebSite.Models;
using CapitalSoftWebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Controllers
{
    
    public class HomeController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var model = new HomePageModel();
                model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == cultureName).ToList();
                model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == cultureName).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Contact contact, string lang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new DbAdaptor().CreateContact(contact);
                }
                var model = new HomePageModel();
                model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == cultureName).ToList();
                model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == cultureName).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        public FileContentResult GetImage(int imageId)
        {
            Image image = new DbAdaptor().GetImage(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }


        public ActionResult SetCulture(string lang)
        {
            lang = CultureHelper.GetImplementedCulture(lang);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = lang;   
            else
            {

                cookie = new HttpCookie("_culture");
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
    }
}