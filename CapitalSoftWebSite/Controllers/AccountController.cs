using CapitalSoftWebSite.Models;
using CapitalSoftWebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapitalSoftWebSite.Controllers
{
    public class AccountController : Controller
    {

        private static int count; 
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginModel, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                bool res = FormsAuthentication.Authenticate(loginModel.Login, loginModel.Password);
                ++count;
                if (res)
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Login, false);
                    return Redirect(returnUrl);
                }
                else
                {
                    if (count > 2)
                    {
                        //ToDo change redirect
                        count = 0;
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View(loginModel);
                }
            }
            return View(loginModel);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}