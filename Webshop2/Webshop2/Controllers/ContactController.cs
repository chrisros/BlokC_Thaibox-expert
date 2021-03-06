﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.mailControllers;

namespace Webshop2.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.H1 = "Contact";
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            DatabaseControllers.ProductDBController prodDBControl = new DatabaseControllers.ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
            return View();
        }
       [HttpPost]
        public ActionResult Mailsend(FormCollection formCollection)
        {
            DatabaseControllers.CategorieDBController catControl = new DatabaseControllers.CategorieDBController();
            ViewBag.categorieen = catControl.haalCatNamenOp();
            DatabaseControllers.ProductDBController prodDBControl = new DatabaseControllers.ProductDBController();
            ViewBag.merkFilters = prodDBControl.getMerken();
            ViewBag.maatFilters = prodDBControl.getMaten();
            ViewBag.geslachtFilters = prodDBControl.getGeslacht();
           MailSendController email = new MailSendController { EmailAdresNaar = "thaiboxexpert@chros.nl", Onderwerp = formCollection["subject"], Bericht = formCollection["message"], emailAdreszender = formCollection["email"], Naam = formCollection["name"] };
            email.SendEmail();
            ViewBag.H1 = "Over ons";
            ViewBag.feedback = "Mail verstuurd, u hoort zo spoedig mogelijk van ons.";
            return View();
        }
    }
}