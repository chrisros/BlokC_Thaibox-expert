using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshop2.Controllers
{
    public class AboutController : Controller
    {
       
        // GET: About
        public ActionResult Index()
        {
            ViewBag.H1 = "Over ons";
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            DatabaseControllers.ProductDBController prodDBControl = new DatabaseControllers.ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            return View();
        }
        

    }
}