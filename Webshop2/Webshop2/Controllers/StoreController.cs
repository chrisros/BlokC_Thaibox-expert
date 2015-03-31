﻿using System;
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

            foreach (Product produ in producten)
            {
                ViewBag.productID = produ.productID;
                ViewBag.productNaam = produ.productNaam;
                ViewBag.productPrijs = produ.productPrijs;
                ViewBag.productMerk = produ.productMerk;
                ViewBag.productDetail = produ.productDetail;
            }
            return View(producten);
        }

        public ActionResult ProductToevoegen()
        {
            ViewBag.H1 = "Toevoegen producten";

            return View();
        }
        public ActionResult ProductToegevoegd(Product product, HttpPostedFileBase file)
        {


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