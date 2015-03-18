using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshop2.Controllers
{
    public class TermsController : Controller
    {
        // GET: Terms
        public ActionResult Index()
        {
            ViewBag.H1 = "Algemene voorwaarden";

            return View();
        }
    }
}