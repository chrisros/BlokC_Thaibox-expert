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
            ViewBag.merkFilters = prodControl.getMerken();
            ViewBag.maatFilters = prodControl.getMaten();
            ViewBag.geslachtFilters = prodControl.getGeslacht();
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
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();

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
            
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            ViewBag.productLijstje = prodDBControl.haalProductGegevensOp();

            return View();
        }
        public ActionResult ProductToegevoegd(Product product, HttpPostedFileBase file, int categorieID, string productGeslacht)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
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
                product.productCat = categorieID;
                product.productGeslacht = productGeslacht;
                ProdDataBase.RegisterProduct(product);
                ProdDataBase.registerUitvoering(product.productMaat, product.productKleur, product.uitvoeringVoorraad);
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
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
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
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
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
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            ViewBag.H1 = "Categorie Toevoegen";
            return View();
        }
        public ActionResult CategorieToegvoegd(Categorie categorie)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
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
            ProductDBController prodDBControl = new ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            //ViewBag.prijs = besteldbcontrol.HaalBestellingTotaalPrijsOp();
            //List<Product> producten = prodControl.haalProductDetailGegevensOp(productID);
            //cat
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
                ProductDBController prodDBControl = new ProductDBController();
                ViewBag.merkFilters = prodDBControl.getMerken();
                ViewBag.maatFilters = prodDBControl.getMaten();
                ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            return View(producten);
        }

        public ActionResult filterProducten(string filterSoort, string filter)
        {
            ProductDBController prodDBControl = new ProductDBController();
            List<Product> producten = prodDBControl.filterProduct(filterSoort, filter);
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            ViewBag.H1 = "Gevonden producten";
            return View(producten);
        }
    }
}