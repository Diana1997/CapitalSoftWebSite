using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapitalSoftWebSite.Models;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    public class ProjectsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View(new DbAdaptor().GetProjects());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public ActionResult Create()
        {
            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
                "Value", "Text");
            ViewBag.TechnologyID = new MultiSelectList(new DbAdaptor().GetTechnologies(), "TechnologyID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project, HttpPostedFileBase[] files, int[] TechnologyID)
        {
            if (ModelState.IsValid)
            {
                DbAdaptor dbAdaptor = new DbAdaptor();
                IList<Image> imageList = new List<Image>();
                Image img = null;
                if(files != null)
                {
                    foreach(var file in files)
                    {
                        img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                        file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                        imageList.Add(img);
                    }
                }
                int projectId = dbAdaptor.CreateProject(new Project {
                     Name = project.Name, 
                     Description = project.Description, 
                     Images = imageList,
                     Lang = project.Lang,
                });
                foreach (var elem in TechnologyID)
                {
                    dbAdaptor.CreateProjectTechnology(new ProjectTechnology
                    {
                        ProjectID = projectId,
                        TechnologyID = elem,
                    });
                }
                return RedirectToAction("Index");
            }
            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
                "Value", "Text");
            return View(project);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProjectID,Name,Description,Lang")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Project project = new DbAdaptor().GetProject(id);

            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new DbAdaptor().DeleteProject(id);
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
