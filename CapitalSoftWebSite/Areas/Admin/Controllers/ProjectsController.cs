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

        public ActionResult Index()
        {
            return View(new DbAdaptor().GetProjects());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Project project = new DbAdaptor().GetProject(id);
            if (project == null)
                return HttpNotFound();

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
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                        file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                        imageList.Add(img);
                    }
                }
                int projectId = dbAdaptor.CreateProject(new Project
                {
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Project project = new DbAdaptor().GetProject(id);
            if (project == null)
                return HttpNotFound();

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");

            var list1 = new DbAdaptor().GetTechnologies()?.ToList();
            var list2 = project.Technologies?.ToList();

            if (list2 != null)
            {
                var list3 = list1.Where(x => !list2.Contains(x)).ToList();
                ViewBag.TechnologyID = new MultiSelectList(list3, "TechnologyID", "Name");
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project, HttpPostedFileBase[] files, int[] TechnologyID)
        {
            if (ModelState.IsValid)
            {
                DbAdaptor dbAdaptor = new DbAdaptor();
                Image img = null;
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        img = new Image { ImageData = new byte[file.ContentLength], ImageMimeType = file.ContentType, };
                        file.InputStream.Read(img.ImageData, 0, file.ContentLength);
                        img.ProjectID = project.ProjectID;
                        dbAdaptor.CreateImage(img);
                    }
                }

                if(TechnologyID != null)
                {
                    foreach (var elem in TechnologyID)
                    {
                        dbAdaptor.CreateProjectTechnology(new ProjectTechnology
                        {
                            ProjectID = project.ProjectID,
                            TechnologyID = elem,
                        });
                    }
                }

                dbAdaptor.EditProject(project);
                return RedirectToAction("Index");
            }

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");
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

        public FileContentResult GetImage(int imageId)
        {
            Image image = new DbAdaptor().GetImage(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }

        public ActionResult DeleteImage(Project project, int imgDeleteID)
        {
            new DbAdaptor().DeleteImage(imgDeleteID);
            return RedirectToAction("Edit", "Projects", new { id = project.ProjectID });
        }

        public ActionResult DeleteTechnology(Project project, int techDeleteID)
        {
            new DbAdaptor().DeleteProjectTechnology(project.ProjectID, techDeleteID);
            return RedirectToAction("Edit", "Projects", new { id = project.ProjectID });
        }
    }
}
