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
        List<Models.Product> producten = new List<Models.Product>();
        List<int> prijzen = new List<int>();
        int aantal = 1;
        public ActionResult Index()
        {
      
            ViewBag.H1 = "Winkelwagen";
            bool ingelogd = false;
            DatabaseControllers.BestellingDBController besteldbcontrol= new DatabaseControllers.BestellingDBController();
            if (Session["LoggedIn"] != null)
            {
                ingelogd = (bool)Session["LoggedIn"];
                ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOpUser();
                producten = besteldbcontrol.haalProductGegevensOpVoorGebruiker();
            }
            ViewBag.loggedin = ingelogd;
            if(Session["LoggedIn"] == null)
            {
                Product p = besteldbcontrol.haalProductGegevensOp(1, aantal);
                for (int i = 0; i < p.productAantal; i++)
                {
                    prijzen.Add(p.productPrijs);
                }
                
                producten.Add(p);

                
                ViewBag.Prijs = sessieTotaalPrijs();

            }
            return View(producten);
        }

        public List<Product> getProductenInSessie(int productID)
        {
            
            return producten;
        }

        public Int32 sessieTotaalPrijs()
        {
            int totaalprijs = 0;
            foreach(int prijs in prijzen)
            {
                totaalprijs = totaalprijs + prijs;
            }
            return totaalprijs;
        }

        public ActionResult toegevoegd(int productID, int aantal)
        {
            DatabaseControllers.BestellingDBController besteldbcontrol = new DatabaseControllers.BestellingDBController();
            List<Product> toegevoegdProd = new List<Product>();
            besteldbcontrol.ProductAanWinkelmandToevoegenGebruiker();
            Product p = besteldbcontrol.haalProductGegevensOp(productID, aantal);
            toegevoegdProd.Add(p);
            producten.Add(p);
            return View(producten);
        }

        public ActionResult updateProductAantal()
        {
            aantal = Convert.ToInt32(Request.Form["aantalBox"]);
            return RedirectToAction("Index");
        }
    }
}