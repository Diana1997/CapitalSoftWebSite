using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    //[Authorize]
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            return View(new DbAdaptor().GetContacts());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contact technology = new DbAdaptor().GetContact(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Contact contact = new DbAdaptor().GetContact(id);
            if (contact == null)
                return HttpNotFound();

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new DbAdaptor().DeleteContact(id);
            return RedirectToAction("Index");
        }
    }
}