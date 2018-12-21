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
    //[Authorize]
    public class ProjectsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            IList<Project> list = await DbAdaptor.GetProjectsAsync();
            return View(list);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Project project = await DbAdaptor.GetProjectAsync(id);
            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
                "Value", "Text");
            IList<Technology> technologies = await DbAdaptor.GetTechnologiesAsync();
            ViewBag.TechnologyID = new MultiSelectList(technologies, "TechnologyID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Project project, HttpPostedFileBase[] files, int[] TechnologyID)
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
                int projectId = await DbAdaptor.CreateProjectAsync(new Project
                {
                    Name = project.Name,
                    Description = project.Description,
                    Images = imageList,
                    Lang = project.Lang,
                });
                foreach (var elem in TechnologyID)
                {
                    await DbAdaptor.CreateProjectTechnologyAsync(new ProjectTechnology
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Project project = await DbAdaptor.GetProjectAsync(id);
            if (project == null)
                return HttpNotFound();

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");

            var all = await DbAdaptor.GetTechnologiesAsync();
            var selected = project.Technologies?.ToList();

            if (selected != null)
            {
                var notSelected = all.Where(x => !selected.Contains(x)).ToList();
                ViewBag.TechnologyID =   notSelected.Count > 0 ?  new MultiSelectList(notSelected, "TechnologyID", "Name") : null;
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Project project, HttpPostedFileBase[] files, int[] TechnologyID)
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
                        await DbAdaptor.CreateImageAsync(img);
                    }
                }

                if(TechnologyID != null)
                {
                    foreach (var elem in TechnologyID)
                    {
                        await DbAdaptor.CreateProjectTechnologyAsync(new ProjectTechnology
                        {
                            ProjectID = project.ProjectID,
                            TechnologyID = elem,
                        });
                    }
                }

                await DbAdaptor.EditProjectAsync(project);
                return RedirectToAction("Index");
            }

            ViewBag.Lang = new SelectList(new List<SelectListItem> {
                    new SelectListItem {Text = "en", Value = "en"},
                    new SelectListItem {Text = "ru", Value = "ru"},
                    new SelectListItem {Text = "am", Value = "am"}, },
            "Value", "Text");
            return View(project);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Project project = await DbAdaptor.GetProjectAsync(id);

            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await  DbAdaptor.DeleteProjectAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<FileContentResult> GetImage(int imageId)
        {
            Image image = await DbAdaptor.GetImageAsync(imageId);
            if (image != null)
                return File(image.ImageData, image.ImageMimeType);
            return null;
        }

        public async Task<ActionResult> DeleteImage(Project project, int imgDeleteID)
        {
            await DbAdaptor.DeleteImageAsync(imgDeleteID);
            return RedirectToAction("Edit", "Projects", new { id = project.ProjectID });
        }

        public async Task<ActionResult> DeleteTechnology(Project project, int techDeleteID)
        {
            await DbAdaptor.DeleteProjectTechnologyAsync(project.ProjectID, techDeleteID);
            return RedirectToAction("Edit", "Projects", new { id = project.ProjectID });
        }
    }
}
