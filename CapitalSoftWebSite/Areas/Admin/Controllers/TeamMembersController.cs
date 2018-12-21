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
    public class TeamMembersController : Controller
    {
        private static int? imageId { set; get; }

        public async Task<ActionResult> Index()
        {
            IList<TeamMember> list =  await DbAdaptor.GetTeamMembersAsync();
            return View(list);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = await DbAdaptor.GetTeamMemberAsync(id);
            if (teamMember == null)
                return HttpNotFound();

            return View(teamMember);
        }

        public ActionResult Create()
        {
            ViewBag.Lang = new SelectList( new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, }, 
                    "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeamMember teamMember, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Image img = null;
                if (file != null)
                {
                    img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                    file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                }

                await DbAdaptor.CreateTeamMemberAsync(new TeamMember
                {
                    Firstname = teamMember.Firstname,
                    Lastname = teamMember.Lastname,
                    Position = teamMember.Position,
                    Lang = teamMember.Lang,
                    Image = img,
                });
                return RedirectToAction("Index");
            }

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
                    "Value", "Text");
            return View(teamMember);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = await DbAdaptor.GetTeamMemberAsync(id);

            imageId = teamMember.Image?.ImageID;

            if (teamMember == null)
                return HttpNotFound();

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");
            return View(teamMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeamMember teamMember, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                TeamMember teamMemberEdit = new TeamMember
                {
                    TeamMemberID = teamMember.TeamMemberID,
                    Firstname = teamMember.Firstname,
                    Lastname = teamMember.Lastname,
                    Position = teamMember.Position,
                    Lang = teamMember.Lang,
                };
                if (file != null)
                {
                    var img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                    file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                    teamMemberEdit.ImageId = await DbAdaptor.CreateImageAsync(img);
                }
                else
                    teamMemberEdit.ImageId = imageId;

                
                await DbAdaptor.EditTeamMemberAsync(teamMemberEdit);
                return RedirectToAction("Index");
            }

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");
            return View(teamMember);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = await DbAdaptor.GetTeamMemberAsync(id);
            if (teamMember == null)
                return HttpNotFound();
            return View(teamMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await DbAdaptor.DeleteTeamMemberAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<FileContentResult> GetImage(int imageId)
        {
            Image image = await DbAdaptor.GetImageAsync(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }

        public async Task<ActionResult> DeleteImage(TeamMember teamMember, int imgDeleteID)
        {
             await DbAdaptor.EditTeamMemberAsync(new TeamMember
            {
                TeamMemberID = teamMember.TeamMemberID,
                Firstname = teamMember.Firstname,
                Lastname = teamMember.Lastname,
                Lang = teamMember.Lang,
                Position = teamMember.Position,
                ImageId = null,
            });
            await DbAdaptor.DeleteTeamMemberAsync(imgDeleteID);
            return RedirectToAction("Edit", "TeamMembers", new { id = teamMember.TeamMemberID});
        }
    }
}
