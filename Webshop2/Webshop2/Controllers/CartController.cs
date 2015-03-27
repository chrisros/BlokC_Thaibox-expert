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
                ViewBag.bestelID = besteldbcontrol.getBestelID();
                return View(productenInDB);
            }
            ViewBag.loggedin = ingelogd;
            if (ingelogd == false)
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
            int totaalprijs = 30000;
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

        public ActionResult updateProductAantal(int productID, int aantal, string kleur, string maat)
        {
            Boolean ingelogd = (bool)Session["Ingelogd"];
            if (ingelogd == null)
            {
                ingelogd = false;
                ViewBag.loggedin = ingelogd;
                var prod = productenInSessie.Where(d => d.productID == productID).FirstOrDefault();
                if (prod != null) { prod.productAantal = aantal; }
            }
            if (ingelogd == true)
            {
                ingelogd = true;
                ViewBag.loggedin = ingelogd;
                DatabaseControllers.BestellingDBController besteldbcontrol = new DatabaseControllers.BestellingDBController();
                int uitvoeringID = besteldbcontrol.haalUitvoeringsIDOp(productID, maat, kleur);
                besteldbcontrol.editAantalWinkelmandGebruiker(aantal, uitvoeringID);

            }
            return RedirectToAction("Index");
        }

        public ActionResult deleteProduct(int productID, string kleur, string maat)
        {
            Boolean ingelogd = (bool)Session["Ingelogd"];
            if (ingelogd == false)
            {
                ingelogd = false;
                ViewBag.loggedin = ingelogd;
                var prod = productenInSessie.Where(d => d.productID == productID).FirstOrDefault();
                if (prod != null) { productenInSessie.Remove(prod); }
                List<Product> test = productenInSessie;
            }
            if (ingelogd == true)
            {
                ingelogd = true;
                ViewBag.loggedin = ingelogd;
                DatabaseControllers.BestellingDBController besteldbcontrol = new DatabaseControllers.BestellingDBController();
                int uitvoeringID = besteldbcontrol.haalUitvoeringsIDOp(productID, maat, kleur);
                besteldbcontrol.deleteWinkelMandProductGebruiker(uitvoeringID);

            }
            return RedirectToAction("Index");
        }
    }
}