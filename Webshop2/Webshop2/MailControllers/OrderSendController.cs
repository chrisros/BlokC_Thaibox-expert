using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace Webshop2.mailControllers
{
    public class OrderSendController : Controller
    {
        public String EmailAdresNaar { get; set; }
        public String emailAdreszender { get; set; }
        public String Onderwerp { get; set; }
        public String Bericht { get; set; }
        public String Bericht2 { get; set; }
        public String Naam { get; set; }

        public void SendEmail()
        {

            //Om te testen moet je een valide emailadresVan invoeren met bijbehorend password.

            string emailAdresVan = "thaiboxexpert@chros.nl";
            string password = "thaibox3xp3rt";

            //MailMessage object aanmaken.
            MailMessage msg = new MailMessage();
            //Properties van het zojuist aangemaakte MailMessage zetten
            msg.From = new MailAddress(emailAdresVan);
            msg.To.Add(new MailAddress(EmailAdresNaar));
            msg.Subject = Onderwerp;
            msg.Body = "Beste medewerker, gebruiker met id :  <b>" + Naam + "</b> heeft een bestelling geplaatst, graag deze zo spoedig mogelijk Verwerken. <br><br><br>" + Bericht + "<br><br>"+Bericht2+"<br><br><br>" + "<b>afzender: </b>" + emailAdreszender;
            msg.IsBodyHtml = true;

            //Als SmtpClient ga ik in dit voorbeeld uit van gmail. Je bent natuurlijk vrij om een ander smtpclient te kiezen. 
            //SmtpClient aanmaken "smtp.gmail.com" (= smtp server gmail) 587 (= Port 587 is voor gebruikers om emails over te versturen.)
            SmtpClient smtpClient = new SmtpClient("smtp.mailplatform.eu", 587);
            //NetworkCredential object aanmaken. Emailadres en password zijn de constructorparameters
            NetworkCredential loginInfo = new NetworkCredential(emailAdresVan, password);

            //Properties van het zojuist aangemaakte SmtpClient zetten
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = loginInfo;
            //Bericht daadwerkelijk versturen
            smtpClient.Send(msg);

        }
    }
}