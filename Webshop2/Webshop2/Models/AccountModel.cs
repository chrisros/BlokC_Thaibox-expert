using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop2.Models
{
    public class AccountModel
    {
        public String Naam { get; set; }
        public String Woonadres { get; set; }
        public String Woonpostcode { get; set; }
        public String Gebruikersnaam { get; set; }
        public String Wachtwoord { get; set; }
        public String Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public Int32 Telefoonnummer { get; set; }
        public String Leveradres { get; set; }
        public String Leverpostcode { get; set; }
    }
}