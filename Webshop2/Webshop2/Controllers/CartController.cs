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
                ingelogd = (bool)Session["LoggedIn"];
                ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOpUser();
                productenInDB = besteldbcontrol.haalProductGegevensOpVoorGebruiker(); 
                return View(productenInDB);
            }
            ViewBag.loggedin = ingelogd;
            if(Session["LoggedIn"] == null)
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

        public List<Product> getProductenInSessie(int productID)
        {
            
            return productenInSessie;
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
            if (Session["LoggedIn"] != null)
            {
                besteldbcontrol.ProductAanWinkelmandToevoegenGebruiker();
                
                return View(productenInDB);
            }
            else if (Session["LoggedIn"] == null)
            {
                productenInSessie.Add(p);
                toegevoegdProd.Add(p);
                return View(toegevoegdProd);
            }
            else { return View(productenInSessie); }
        }

        public ActionResult updateProductAantal()
        {
            aantal = Convert.ToInt32(Request.Form["aantalBox"]);
            return RedirectToAction("Index");
        }
    }
}