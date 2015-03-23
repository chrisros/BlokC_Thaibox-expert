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
            return View();
        }
        

    }
}