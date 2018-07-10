using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapitalSoftWebSite.Models;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    [Authorize]
    public class TeamMembersController : Controller
    {
        private static int? imageId { set; get; }

        public ActionResult Index()
        {
            return View(new DbAdaptor().GetTeamMembers());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = new DbAdaptor().GetTeamMember(id);
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
        public ActionResult Create(TeamMember teamMember, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Image img = null;
                if (file != null)
                {
                    img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                    file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                }

                new DbAdaptor().CreateTeamMember(new TeamMember
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = new DbAdaptor().GetTeamMember(id);

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
        public ActionResult Edit(TeamMember teamMember, HttpPostedFileBase file)
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
                    teamMemberEdit.ImageId = new DbAdaptor().CreateImage(img);
                }
                else
                    teamMemberEdit.ImageId = imageId;

                
                new DbAdaptor().EditTeamMember(teamMemberEdit);
                return RedirectToAction("Index");
            }

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");
            return View(teamMember);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TeamMember teamMember = new DbAdaptor().GetTeamMember(id);
            if (teamMember == null)
                return HttpNotFound();
            return View(teamMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new DbAdaptor().DeleteTeamMember(id);
            return RedirectToAction("Index");
        }

        public FileContentResult GetImage(int imageId)
        {
            Image image = new DbAdaptor().GetImage(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }

        public ActionResult  DeleteImage(TeamMember teamMember, int imgDeleteID)
        {
            new DbAdaptor().EditTeamMember(new TeamMember
            {
                TeamMemberID = teamMember.TeamMemberID,
                Firstname = teamMember.Firstname,
                Lastname = teamMember.Lastname,
                Lang = teamMember.Lang,
                Position = teamMember.Position,
                ImageId = null,
            });
            new DbAdaptor().DeleteImage(imgDeleteID);
            return RedirectToAction("Edit", "TeamMembers", new { id = teamMember.TeamMemberID});
        }
    }
}
