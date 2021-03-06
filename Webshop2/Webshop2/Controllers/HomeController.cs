﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.DatabaseControllers;
using Webshop2.Models;

namespace Webshop2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Product> prod = new List<Product>();
            Session["sessietest"] = prod;
            Session["Ingelogd"] = false;
            Session["SessionExists"] = 1;
            ViewBag.H2 = "Welkom";

            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            DatabaseControllers.ProductDBController prodDBControl = new DatabaseControllers.ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();

            List<Product> uitgelicht = prodDBControl.uitgelichteProducten();

            DatabaseControllers.ReactieDBController reactieDBControll = new DatabaseControllers.ReactieDBController();
            string reactietabel = reactieDBControll.getReactieTable();
            ViewBag.reacties = reactietabel;
            return View(uitgelicht);
        }

        [HttpPost]
        public ActionResult PlaatsReactie(FormCollection formCollection)
        {
            DatabaseControllers.ReactieDBController reactieDBControll = new DatabaseControllers.ReactieDBController();
            int rating = Int32.Parse(formCollection["rating"]);
            
            int gebruikerID = 11;
           // als je hier zorgt dat session name een value heeft werk de sessie gebruiker herkening
            if (formCollection["sessionname"] != null) {gebruikerID = Int32.Parse(formCollection["sessionname"]); };
            reactieDBControll.RegisterReactie(gebruikerID, formCollection["name"], formCollection["message"], rating);
            return RedirectToAction("index");
            
        }   
    }
}