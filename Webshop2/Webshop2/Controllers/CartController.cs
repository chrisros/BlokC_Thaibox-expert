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
        
        List<Models.Product> productenInDB = new List<Models.Product>();
        List<int> prijzen = new List<int>();
        List<Models.Product> productenInSessie = (List<Product>)System.Web.HttpContext.Current.Session["sessietest"];
        int aantal = 1;
        public ActionResult Index()
        {

            ViewBag.H1 = "Winkelwagen";
            bool ingelogd = false;
            DatabaseControllers.BestellingDBController besteldbcontrol= new DatabaseControllers.BestellingDBController();
            if (Session["LoggedIn"] != null)
            {
                ingelogd = (bool)Session["Ingelogd"];
                ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOpUser();
                productenInDB = besteldbcontrol.haalProductGegevensOpVoorGebruiker(); 
                return View(productenInDB);
            }
            ViewBag.loggedin = ingelogd;
            if (Session["Ingelogd"] == null)
            {
                Product p = new Product();
                if(productenInSessie != null)
                {
                    foreach (Product sesprod in productenInSessie)
                    {
                    prijzen.Add(p.productPrijs);
                    }
                }
                                
                ViewBag.Prijs = sessieTotaalPrijs();
                return View(productenInSessie);
            }
            return View(productenInSessie);
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
            Product p = besteldbcontrol.haalProductGegevensOp(productID, aantal);
            if (Session["Ingelogd"] != null)
            {
                besteldbcontrol.productToevoegenWinkelWagenGebruiker(aantal, 1);
                toegevoegdProd.Add(p);
                prijzen.Add(p.productPrijs);

                return View(toegevoegdProd);
            }
            else if (Session["Ingelogd"] == null)
            {
                productenInSessie.Add(p);
                toegevoegdProd.Add(p);
                return View(toegevoegdProd);
            }
            else { return View(productenInSessie); }
        }

        public ActionResult updateProductAantalSessie(int aantal, int productID)
        {
                var product = productenInSessie.Where(d => d.productID == productID).First();
                if (product != null) { product.productAantal = aantal; }
            return RedirectToAction("Index");
        }

        public ActionResult deleteProductInWinkelwagen()
        {

            return RedirectToAction("Index");
        }
    }
}