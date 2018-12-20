using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Areas.Admin.Controllers
{
    //[Authorize]
    public class ContactsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var contact = await new DbAdaptor().GetContactsAsync();
            return View();
        }

        public async Task<ActionResult>  Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contact technology = await  new DbAdaptor().GetContactAsync(id);
            if (technology == null)
                return HttpNotFound();

            return View(technology);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Contact contact = await  new DbAdaptor().GetContactAsync(id);
            if (contact == null)
                return HttpNotFound();

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await new DbAdaptor().DeleteContactAsync(id);
            return RedirectToAction("Index");
        }
    }
}