using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshop2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            ViewBag.H1 = "Log in!";
            return View();
        }
        public ActionResult Admin()
        {
            ViewBag.H1 = "Beheer login";
            return View();
        }
        public ActionResult create()
        {
            ViewBag.H1 = "Account creëren";
            return View();
        }
    }
}