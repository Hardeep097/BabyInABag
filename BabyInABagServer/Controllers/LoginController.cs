using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Models.Repos;

namespace BabyInABagServer.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseType(typeof(CustomerAdminLogin))]
        public ActionResult Login(CustomerAdminLogin cal)
        {
            if (ModelState.IsValid)
            {
                LoginRepository la = new LoginRepository();
                string authResponse = la.LoginAuthenticate(cal);

                ViewBag.LoginResponse = authResponse;

                if (authResponse.Equals("Login Success"))
                    return RedirectToAction("Index", "Home");
                else if(authResponse.Equals("Admin Login Success"))
                    return RedirectToAction("Orders", "Admin");
                else
                    return View();
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}