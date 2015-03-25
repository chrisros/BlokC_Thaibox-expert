using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.Models;
using Webshop2.DatabaseControllers;
using MySql.Data.MySqlClient;

namespace Webshop2.Controllers
{
    public class StoreController : Controller
    {

        static ProductDBController ProdDataBase = new ProductDBController();

        // GET: Store
        public ActionResult Index()
        {
            ViewBag.H1 = "Winkel";
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalProductGegevensOp();
            return View(producten);
        }

        public ActionResult ProductDetail(int productID)
        {
            ViewBag.H1 = "Product Detail";

            Models.Product p = new Models.Product();

            if (productID == 1)
            {
                p.productID = productID;
                p.productNaam = "Sport broek";
                p.productDetail = "Strakke sport broek met wat leuke kleurtjes";
            }
            else if (productID == 2)
            {
                p.productID = productID;
                p.productNaam = "Hesje";
                p.productDetail = "Geel hesje met reflectie materiaal";
            }
            else if (productID == 3)
            {
                p.productID = productID;
                p.productNaam = "3";
                p.productDetail = "3";
            }
            else if (productID == 4)
            {
                p.productID = productID;
                p.productNaam = "4";
                p.productDetail = "4";
            }
            else if (productID == 5)
            {
                p.productID = productID;
                p.productNaam = "5";
                p.productDetail = "5";
            }
            else if (productID == 6)
            {
                p.productID = productID;
                p.productNaam = "6";
                p.productDetail = "6";
            }

            else
            {
                p.productNaam = "Onbekend";
                p.productDetail = "Onbekend";
            }

            return View(p);
        }

        public ActionResult ProductToevoegen()
        {
            ViewBag.H1 = "Toevoegen producten";

            return View();
        }

        public ActionResult ProductToegevoegd(Product product)
        {
            ViewBag.H1 = "Account geregistreerd.";

            if (ModelState.IsValid)
            {
                ProdDataBase.RegisterProduct(product);
                return View();
            }
            else
            {
                return View("ProductToevoegen", product);
            }
        }

        public ActionResult Kleding()
        {
            ViewBag.H1 = "Kleding";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalKledingGegevensOp();
            return View(producten);
        }

        public ActionResult Shirts()
        {
            ViewBag.H1 = "Shirts";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalShirtsOp();
            return View(producten);
        }


    }
}