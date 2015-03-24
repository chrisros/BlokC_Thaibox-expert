using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.DatabaseControllers;
using Webshop2.Models;

namespace Webshop2.Controllers
{
    public class AccountController : Controller
    {
        static RegisterDBController RegDB = new RegisterDBController();
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
        public ActionResult RegCompleted(AccountModel account)
        {
            ViewBag.H1 = "Account geregistreerd.";

            if (ModelState.IsValid) { 
            RegDB.RegisterAccount(account);
            return View();
            }
            else
            {
                return View("create", account);
            }
            
        }
        public ActionResult Login(string username, string password)
        {
            AccountModel account = RegDB.LoginCheck(username, password);

            bool mailklopt = account.Email.Equals(username);
            bool passklopt = account.Wachtwoord.Equals(password);

            if (!mailklopt || !passklopt) {
                return View("Index"); 
            }
            else
            {
                Session["LoggedIn"] = true;
                return View(account);
            }
        }
    }
}