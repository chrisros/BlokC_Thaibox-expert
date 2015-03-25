using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop2.Models
{
    public class AdminModel
    {
        public int GebruikerID { get; set; }
        public String Naam { get; set; }
        public String email { get; set; }
        public String Wachtwoord { get; set; }
        public String Functie { get; set; }
    }
}