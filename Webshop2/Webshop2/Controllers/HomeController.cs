using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.Models;

namespace Webshop2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Product> prod = new List<Product>();
            Product p = new Product { productID = 1, productNaam = "testnaam", productMerk = "testmerk", productPrijs = 10000, productDetail = "hoi", productAantal = 1 };
            prod.Add(p);
            Product p1 = new Product { productID = 2, productNaam = "professionele hoed", productMerk = "testmerk2", productPrijs = 20000, productDetail = "hoi2", productAantal = 1 };
            prod.Add(p1);
            Session["sessietest"] = prod;
            Session["Ingelogd"] = false;
            Session["SessionExists"] = 1;
            ViewBag.H2 = "Welkom";

            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();

            DatabaseControllers.ReactieDBController reactieDBControll = new DatabaseControllers.ReactieDBController();
            string reactietabel = reactieDBControll.getReactieTable();
            ViewBag.reacties = reactietabel;
            return View();
        }
    }
}