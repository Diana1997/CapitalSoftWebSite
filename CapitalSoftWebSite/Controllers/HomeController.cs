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
            try
            {
                model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == cultureName).ToList();
                model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == cultureName).ToList();
                ViewBag.SendMessage = "";
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Contact contact, string lang)
        {
            var model = new HomePageModel();
            try
            {
                if (ModelState.IsValid)
                {
                    if(new DbAdaptor().CreateContact(contact) > 0)
                    {
                        ViewBag.SendMessage = Resources.Resource.Message_sent; 
                        ModelState.Clear();
                    }
                }
                model.TeamMembers = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == cultureName).ToList();
                model.Projects = new DbAdaptor().GetProjectsFull().Where(x => x.Lang == cultureName).ToList();
            }
            catch (Exception)
            {
                ViewBag.SendMessage = Resources.Resource.Message_not_sent;
            }
            return View(model);
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