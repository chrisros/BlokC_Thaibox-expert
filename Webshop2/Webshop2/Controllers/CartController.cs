using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.Models;

namespace Webshop2.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
           List<Models.Product> producten = new List<Models.Product>();
            ViewBag.H1 = "Winkelwagen";
            bool ingelogd = false;
            DatabaseControllers.BestellingDBController besteldbcontrol= new DatabaseControllers.BestellingDBController();
            besteldbcontrol.berekenTotaalPRijs();
            ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            producten = besteldbcontrol.haalProductGegevensOp();
            if (Session["LoggedIn"] != null)
            {
                ingelogd = (bool)Session["LoggedIn"];
            }
            ViewBag.loggedin = ingelogd;
           // List<Product> producten = (List<Product>)Session["sessietest"];
            //if (System.Web.HttpContext.Current.Session["Sessionexists"] != null)
           // {
            //    Session["Sessionexists"] = 0;
            //}
            return View(producten);
        }
    }
}