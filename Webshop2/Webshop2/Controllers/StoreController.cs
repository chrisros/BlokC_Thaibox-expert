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
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Product> producten = prodControl.haalProductDetailGegevensOp(productID);
            ViewBag.H1 = "Product Detail";

            return View(producten);
        }

        public ActionResult ProductToevoegen()
        {
            ViewBag.H1 = "Toevoegen producten";

            return View();
        }
        public ActionResult ProductToegevoegd(Product product, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("/productImages"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    file.InputStream.CopyTo(ms);
                //    byte[] array = ms.GetBuffer();
                //}

            }
            ViewBag.H1 = "Product geregistreerd.";

            if (ModelState.IsValid)
            {
                ProdDataBase.RegisterProduct(product);
                return View();
            }
            else
            {
                return View("ProductToevoegen", product);
            }
            //afhendelen van de foto upload
            
        }
        public ActionResult ProductWijzigen()
        {
            ViewBag.H1 = "Wijzigen van producten";
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalProductGegevensOp();
            return View(producten);
        }
        public ActionResult ProductGeWijzigd(Product product)
        {
            ViewBag.H1 = "Product is gewzijgid";
            if (ModelState.IsValid)
            {
                ProdDataBase.WijzigenProduct(product);
                return View();
            }
            else
            {
                return View("ProductWijzigen", product);
            }
        }

        public FileContentResult getImage()
        {
            Product product = ProdDataBase.getImageOutDB();
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult UpdateImage(Product product, HttpPostedFileBase image)
        {

            if (image != null)
            {
                product.ImageMimeType = image.ContentType;
                product.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(product.ImageData, 0, image.ContentLength);
            }
            else
            {
                //plaatje ophalen van bijbehorend paspoort.
                Product product2 = ProdDataBase.getImageOutDB();
                if (product2 != null)
                {
                    product.ImageData = product2.ImageData;
                }

            }

            if (ModelState.IsValid)
            {
                ProdDataBase.RegisterProduct(product);
                return Redirect("ProductToevoegen");
            }
            else
            {
                return View("ProductToevoegen", product);
            }

        }

        public ActionResult UploadImage(Product product)
        {
            HttpPostedFileBase file = Request.Files["fileuploadImage"];

            // write your code to save image
            string uploadPath = Server.MapPath("~/images/");
            file.SaveAs(uploadPath + file.FileName);

            return View("Store", product);
        }

        // Kleding
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
        public ActionResult Schoenen()
        {
            ViewBag.H1 = "Schoenen";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalSchoenenOp();
            return View(producten);
        }
        public ActionResult Broeken()
        {
            ViewBag.H1 = "Broeken";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalBroekenOp();
            return View(producten);
        }
        public ActionResult Sokken()
        {
            ViewBag.H1 = "Sokken";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalSokkenOp();
            return View(producten);
        }
        public ActionResult Ondergoed()
        {
            ViewBag.H1 = "Ondergoed";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalOndegoedOp();
            return View(producten);
        }

        // Bescherming

        public ActionResult Bescherming()
        {
            ViewBag.H1 = "Bescherming";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalBeschermingGegevensOp();
            return View(producten);
        }
        public ActionResult Hoofdbescherming()
        {
            ViewBag.H1 = "Hoofdbescherming";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalHoofdOp();
            return View(producten);
        }
        public ActionResult Borstbescherming()
        {
            ViewBag.H1 = "Borstbescherming";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalBorstOp();
            return View(producten);
        }
        public ActionResult Beenbescherming()
        {
            ViewBag.H1 = "Beenbescherming";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalBeenOp();
            return View(producten);
        }
        public ActionResult Handbescherming()
        {
            ViewBag.H1 = "Handbescherming";
            DatabaseControllers.CategorieDBController prodControl = new DatabaseControllers.CategorieDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalHandOp();
            return View(producten);
        }


    }
}