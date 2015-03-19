using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.Models;

namespace Webshop2.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            ViewBag.H1 = "Winkel";

            return View();
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
            else
            {
                p.productNaam = "Onbekend";
                p.productDetail = "Onbekend";
            }

            return View(p);
        }
    }
}