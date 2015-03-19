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
        public ActionResult RegCompleted(String username, String surname, String adress, String postal, String location, String tel, String mail, String password1, String password2)
        {
            ViewBag.H1 = "Account geregistreerd";

            AccountModel account = new AccountModel();
            account.Email = mail;
            account.Wachtwoord = password1;
            account.Gebruikersnaam = username;
            account.Naam = surname;
            account.Woonadres = adress;
            account.Woonpostcode = postal;
            account.Telefoonnummer = Int32.Parse(tel);

            /* wachtwoord checken voor submit, kan later?
                 * 
                 * if(password1.Equals(password2)){
                    WachtPara.Value = password1;
                }
                else{
                    //FoutMelding
                }*/

            RegDB.RegisterAccount(account);

            
            return View();
        }
    }
}