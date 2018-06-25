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
    public class TeamMembersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View(new DbAdaptor().GetTeamMember());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        public ActionResult Create()
        {
            ViewBag.ImageId = new SelectList(db.Images, "ImageID", "ImageMimeType");
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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageId = new SelectList(db.Images, "ImageID", "ImageMimeType", teamMember.ImageId);
            return View(teamMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamMemberID,Firstname,Lastname,Position,LanguageID,ImageId")] TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teamMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageId = new SelectList(db.Images, "ImageID", "ImageMimeType", teamMember.ImageId);
            return View(teamMember);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeamMember teamMember = db.TeamMembers.Find(id);
            db.TeamMembers.Remove(teamMember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
