using CapitalSoftWebSite.Filters;
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
    
    public class HomeController : Controller
    {
        [Culture]
        public ActionResult Index()
        {
            var model = new HomePageModel();
            model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            model.Projects = new DbAdaptor().GetProjectsHome().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            return View(model);
        }

        [Culture]
        public ActionResult ChangeCulture(string lang)
        {
            ChangeLang(lang);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(Contact contact, string lang)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppDbContext(ConnectionParameters.connectionString))
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                }
                return RedirectToAction("index");
            }
            return View("~/Views/Home/Index.cshtml", new HomePageModel { Contact = contact });
        }

        private void ChangeLang(string lang)
        {
            List<string> cultures = new List<string>() { "ru", "en", "am" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
        }

        public FileContentResult GetImage(int imageId)
        {
            Image image = new DbAdaptor().GetImage(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }
    }
}