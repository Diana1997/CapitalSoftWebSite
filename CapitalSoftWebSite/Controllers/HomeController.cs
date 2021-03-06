﻿using CapitalSoftWebSite.Helpers;
using CapitalSoftWebSite.Models;
using CapitalSoftWebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Controllers
{
    
    public class HomeController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new HomePageModel();
            model.TeamMembers = await DbAdaptor.GetTeamMembersAsync(cultureName);
            model.Projects = await  DbAdaptor.GetProjectsFullAsync(cultureName);
            ViewBag.SendMessage = "";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Contact contact, string lang)
        {
            var model = new HomePageModel();
            try
            {
                if (ModelState.IsValid)
                {
                    int b =  await DbAdaptor.CreateContactAsync(contact);
                    if (b > 0)
                    {
                        //ToDo check int 
                        ViewBag.SendMessage = Resources.Resource.Message_sent; 
                        ModelState.Clear();
                    }
                }
                model.TeamMembers = await DbAdaptor.GetTeamMembersAsync(cultureName);
                model.Projects = await DbAdaptor.GetProjectsFullAsync(cultureName);
            }
            catch (Exception)
            {
                ViewBag.SendMessage = Resources.Resource.Message_not_sent;
            }
            return View(model);
        }

        public async Task<FileContentResult> GetImage(int imageId)
        {
            Image image = await DbAdaptor.GetImageAsync(imageId);
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

        [HttpGet]
        public async Task<ActionResult> More(int id)
        {
            Project model = await DbAdaptor.GetProjectAsync(id);
            if (model == null)
                return HttpNotFound();
            return PartialView("_More", model);
        }
    }
}