using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop2.mailControllers;

namespace Webshop2.Controllers
{
    public class CheckoutController : Controller
    {
        DatabaseControllers.ordermailDBController ordermailDBControll = new DatabaseControllers.ordermailDBController();
        // GET: Checkout
        public ActionResult Index()
        {
            string orderid = Request.QueryString["orderid"];
            ViewBag.orderid = orderid;
            ViewBag.H1 = "Afrekenen";
            return View();
        }

        //Deze methode wordt uitgevoerd na het 'betalen'//
        public ActionResult Succes()
        {          

            string orderid = Request.QueryString["orderid"];
            int userid = ordermailDBControll.getorderperson(orderid);
            //invullen van variabelen voor mail
            string onderwerp = "Bestelling met ordernummer: "+orderid ;   
            string mail = "thaiboxexpert@chros.nl";
            string naam = userid.ToString();
            string bericht = "<table>" + ordermailDBControll.getbestelling(orderid)+"</table>";
            string bericht2 = "<table>" + ordermailDBControll.getPersoonGegevens(userid)+"</table>";                          
            OrderSendController email = new OrderSendController { 
                EmailAdresNaar =     "thaiboxexpert@chros.nl", 
                Onderwerp =          onderwerp, 
                Bericht =            bericht, 
                Bericht2 =           bericht2,
                emailAdreszender =   mail, 
                Naam =               naam 
            };

            email.SendEmail();


            if (ordermailDBControll.isGoldCustomer(userid)==true && ordermailDBControll.knownGoldCustomer(userid)==false)
            {
                ordermailDBControll.updateGoldCustomerStatus(userid);
                //invullen van variabelen voor mail
            onderwerp = "U bent GOLD-Customer"+orderid ;   
            mail = "thaiboxexpert@chros.nl";
            naam = ordermailDBControll.getNaam(userid);
            string EmailAdres = ordermailDBControll.getEmail(userid);
            bericht = " Omdat u al meer dan 500 euro aan producten heeft besteld bent u vanaf heden gold customer. <br>Als gold customer krijgt u 4% korting op het gehele assortiment";

            GoldSendController email2 = new GoldSendController
            {
                EmailAdresNaar = EmailAdres,
                Onderwerp = onderwerp,
                Bericht = bericht,
                emailAdreszender = mail,
                Naam = naam

            };
            email2.SendEmail();
            }

            ViewBag.H1 = "Bestelling compleet!";
            ViewBag.H2 = "order met id: " + orderid+"is verstuurd en wordt z.s.m afgehandeld";
            return View();
        }
    }
}