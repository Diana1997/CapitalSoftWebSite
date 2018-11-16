using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Controllers
{
    public class TeamController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var team = new DbAdaptor().GetTeamMembers().Where(x => x.Lang == cultureName)?.ToList();
            return View(team);
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