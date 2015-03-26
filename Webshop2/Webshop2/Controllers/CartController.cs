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
            {
            List<Product> prod = new List<Product>();
            Product p = new Product { productID = 1, productNaam = "testnaam", productMerk = "testmerk", productPrijs = 10000, productDetail = "hoi", productAantal = 1 };
            prod.Add(p);
            Product p1 = new Product { productID = 2, productNaam = "Thaibox handschoenen extremo", productMerk = "testmerk2", productPrijs = 20000, productDetail = "hoi2", productAantal = 1 };
            prod.Add(p1);
            Session["sessietest"] = prod;
            Session["SessionExists"] = 1;
            }
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
                foreach (Product sesprod in productenInSessie)
                {
                    prijzen.Add(p.productPrijs);
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

<<<<<<< HEAD
        public ActionResult updateProductAantalSessie(int aantal, int productID)
        {
                //var product = productenInSessie.Where(d => d.productID == productID).First();
                //if (product != null) { product.productAantal = aantal; }

                var i = productenInSessie.FindIndex(x => x.productID == productID);
                productenInSessie[i].productAantal = aantal;
            return RedirectToAction("Index");
        }

        public ActionResult deleteProductInWinkelwagen()
=======
        public ActionResult updateProductAantal()
>>>>>>> parent of 2c77894... aantal aanpassen in winkelmandje bij sessie
        {
            aantal = Convert.ToInt32(Request.Form["aantalBox"]);
            return RedirectToAction("Index");
        }

        public ActionResult updateProductAantalGebruikerInDB(int aantal, int productID, string maat, string kleur)
        {
            DatabaseControllers.BestellingDBController besteldbcontrol = new DatabaseControllers.BestellingDBController();

            besteldbcontrol.editAantalInDB(aantal, productID, maat, kleur); 
            
            return RedirectToAction("Index");
        }
    }
}