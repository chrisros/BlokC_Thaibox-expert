using MySql.Data.MySqlClient;
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
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            return View();
        }
        public ActionResult Admin()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Beheer login";
            return View();
        }
        public ActionResult AdminLogin(string username, string password) {

            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Ingelogd als medewerker";
            AdminModel admin = RegDB.AdminLoginCheck(username);
            
            bool userklopt = true;
            bool passklopt = true;

            if (admin.Naam == null)
            {
                userklopt = false;
            }
            else
            {
                if (!admin.Naam.Equals(username))
                {
                    userklopt = false;
                }
            }
            if (admin.Wachtwoord == null)
            {
                passklopt = false;
            }
            else
            {
                if (!admin.Wachtwoord.Equals(password))
                {
                    passklopt = false;
                }
            }

            if (!userklopt || !passklopt)
            {
                ViewData["AdminLogError"] = "Uw gegevens zijn onjuist. Probeer het opnieuw.";
                return View("Admin");
            }
            else
            {
                Session["AdminLoggedIn"] = admin.Naam;
                return View(admin);
            }
        }
        public ActionResult create()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Account creëren";
            return View();
        }
        public ActionResult RegCompleted(AccountModel account)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Account geregistreerd.";

            if (ModelState.IsValid && RegDB.isNewEmail(account.Email))
            { 
            RegDB.RegisterAccount(account);
            ViewBag.H1 = "Account geregistreerd.";
            ViewBag.H2 = "";
            return View();
            }
            else if (!RegDB.isNewEmail(account.Email))
            {
                ViewBag.H1 = "Account creëren";
                ViewBag.H2 = "Dit Email adres is reeds geregistreerd.";
                return View("create", account);
            }
            else
            {
                ViewBag.H2 = "";
                return View("create", account);

            }
            
        }
        public ActionResult Login(string username, string password)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Ingelogd";
            AccountModel account = RegDB.LoginCheck(username);
            bool mailklopt = true;
            bool passklopt = true;

            if (account.Email == null)
            {
                mailklopt = false;
            }
            else
            {
                if (!account.Email.Equals(username)) {
                    mailklopt = false;
                }
            } 
            if (account.Wachtwoord == null)
            {
                passklopt = false;
            }
            else
            {
                if (!account.Wachtwoord.Equals(password))
                {
                    passklopt = false;
                }
            }

            if (!mailklopt || !passklopt)
            {
                ViewData["LogError"] = "Uw gegevens zijn onjuist. Probeer het opnieuw.";
                return View("Index");
            }
            else
            {
                Session["LoggedIn"] = account.Email;
                Session["Ingelogd"] = true;
                Session["gebruikerID"] = account.GebruikerID;
                return View(account);
            }
        }

        public ActionResult Logout()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.H1 = "Uitgelogd.";
            Session["LoggedIn"] = null;
            Session["AdminLoggedIn"] = null;
            return View();
        }

        public ActionResult BeheerPagina() 
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            if (Session["AdminLoggedIn"] != null)
            {
                String naam = (String)Session["AdminLoggedIn"];
                AdminModel admin = RegDB.AdminLoginCheck(naam);
                ViewBag.H1 = "Sitebeheer.";
                return View(admin);
            }
            else
            {
                ViewBag.H1 = "Niet ingelogd";
                return View("LoginFout");
            }
        }
        public ActionResult ProfielPagina()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            if (Session["LoggedIn"] != null)
            {
                String email = (String)Session["LoggedIn"];
                AccountModel account = RegDB.LoginCheck(email);
                ViewBag.H1 = "Profielpagina.";
                return View(account);
            }
            else 
            {
                ViewBag.H1 = "Niet ingelogd";
                return View("LoginFout");
            }
        }
        public ActionResult betaaldeProducten()
        {
            BestellingDBController  besteldbcontrol = new BestellingDBController();
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            List<Product> producten = besteldbcontrol.getBetaaldeProducten();
            return View(producten);
        }
        public ActionResult Overzichtpagina()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            if (Session["AdminLoggedIn"] != null)
            {
                String naam = (String)Session["AdminLoggedIn"];
                AdminModel admin = RegDB.AdminLoginCheck(naam);
                ViewBag.H1 = "Financieel overzicht.";
                if (admin.Functie.Equals("manager"))
                {
                    List<Bestelling> overzicht = RegDB.MaandOverzicht();
                    int inkomst = 0;
                    foreach (Bestelling b in overzicht)
                    {
                        inkomst = inkomst + b.totaalPrijs;
                    }
                    ViewData["inkomst"] = inkomst;
                    return View(overzicht);
                }
                else
                {
                    ViewBag.H1 = "Niet ingelogd";
                    return View("LoginFout");
                }
                
            }
            else 
            {
                ViewBag.H1 = "Niet ingelogd";
                return View("LoginFout");
            }
            
        }
        public ActionResult GoedVerkocht() 
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            if (Session["AdminLoggedIn"] != null)
            {
                String naam = (String)Session["AdminLoggedIn"];
                AdminModel admin = RegDB.AdminLoginCheck(naam);
                ViewBag.H1 = "Overzicht van goedverkochte producten.";
                if (admin.Functie.Equals("manager"))
                {
                    List<Product> verkocht = RegDB.ProductOverzicht();
                    return View(verkocht);
                }
                else
                {
                    ViewBag.H1 = "Niet ingelogd";
                    return View("LoginFout");
                }

            }
            else
            {
                ViewBag.H1 = "Niet ingelogd";
                return View("LoginFout");
            }
        }
    }
}