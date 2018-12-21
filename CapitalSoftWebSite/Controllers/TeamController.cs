using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Controllers
{
    public class TeamController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var team = await DbAdaptor.GetTeamMembersAsync(cultureName); 
            return View(team);
        }

        public async Task<FileContentResult> GetImage(int imageId)
        {
            Image image = await DbAdaptor.GetImageAsync(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }
    }
}