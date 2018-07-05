using CapitalSoftWebSite.Filters;
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
            var model = new HomePageModel();
            model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Contact contact, string lang)
        {
            if (ModelState.IsValid)
            {
                new DbAdaptor().CreateContact(contact);
            }
            var model = new HomePageModel();
            model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
            return View(model);
        }

        //[Culture]
        //public ActionResult ChangeCulture(string lang)
        //{
        //    ChangeLang(lang);
        //    return RedirectToAction("Index");
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Culture]
        //public ActionResult Send(Contact contact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        new DbAdaptor().CreateContact(contact);
        //        return RedirectToAction("index");
        //    }

        //    var model = new HomePageModel();
        //    model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
        //    model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == CultureAttribute.cultureName).ToList();
        //    model.Contact = contact;
        //    return View("~/Views/Home/Index.cshtml", model);
        //}

        //private void ChangeLang(string lang)
        //{
        //    List<string> cultures = new List<string>() { "ru", "en", "am" };
        //    if (!cultures.Contains(lang))
        //    {
        //        lang = "en";
        //    }
        //    HttpCookie cookie = Request.Cookies["lang"];
        //    if (cookie != null)
        //        cookie.Value = lang;
        //    else
        //    {

        //        cookie = new HttpCookie("lang");
        //        cookie.HttpOnly = false;
        //        cookie.Value = lang;
        //        cookie.Expires = DateTime.Now.AddYears(1);
        //    }
        //    Response.Cookies.Add(cookie);
        //}

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