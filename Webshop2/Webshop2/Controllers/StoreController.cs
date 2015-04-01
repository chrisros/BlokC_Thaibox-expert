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
        static CategorieDBController CatDataBase = new CategorieDBController();
        public string categorieParameter;

        // GET: Store
        public ActionResult Index()
        {
            ViewBag.H1 = "Winkel";
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalProductGegevensOp();
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            return View(producten);
        }

        public ActionResult ProductDetail(int productID)
        {
            ViewBag.H1 = "Product Detail";
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Product> producten = prodControl.haalProductDetailGegevensOp(productID);
            ViewBag.H1 = "Product Detail";
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();

            foreach (Product produ in producten)
            {
                ViewBag.productID = produ.productID;
                ViewBag.productNaam = produ.productNaam;
                ViewBag.productPrijs = produ.productPrijs;
                ViewBag.productMerk = produ.productMerk;
                ViewBag.productDetail = produ.productDetail;
                ViewBag.productAfbeelding = produ.productAfbeelding;
            }
            return View(producten);
        }

        public ActionResult ProductToevoegen()
        {
            ViewBag.H1 = "Toevoegen producten";
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();

            return View();
        }
        public ActionResult ProductToegevoegd(Product product, HttpPostedFileBase file)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("/productImages"), pic);
                    // file is uploaded
                    product.ImageData = pic;
                    file.SaveAs(path);

                }
                ViewBag.H1 = "Product geregistreerd.";
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
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ViewBag.H1 = "Wijzigen van producten";
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            List<Models.Product> producten = prodControl.haalProductGegevensOp();
            return View(producten);
        }
        public ActionResult ProductGeWijzigd(Product product)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
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
        public ActionResult CategorieToevoegen()
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ViewBag.H1 = "Categorie Toevoegen";
            return View();
        }
        public ActionResult CategorieToegvoegd(Categorie categorie)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            if (ModelState.IsValid)
            {
                ViewBag.H1 = "Categorie Toegevoegd.";
                CatDataBase.voegCatToe(categorie);
                return View();
            }
            else
            {
                return View("CategorieToevoegen", categorie);
            }
        }





        public ActionResult Cat(string catNaam)
        {
            ViewBag.H1 = catNaam;
            DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            //List<Product> producten = prodControl.haalProductDetailGegevensOp(productID);
            //cat
            categorieParameter = catNaam;
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();

            List<Models.Product> producten = catControl.haalGegevensOpvanCat(catNaam);  
            return View(producten);
        }








        

        public ActionResult zoekProducten(string zoekTerm)
        {
            ViewBag.H1 = "Gevonden Producten";
                DatabaseControllers.ProductDBController prodControl = new DatabaseControllers.ProductDBController();
                //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
                List<Models.Product> producten = prodControl.zoekProduct(zoekTerm);
                DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
                ViewBag.categorieen = catControl.haalCatNamenOp();
            return View(producten);
        }
    }
}