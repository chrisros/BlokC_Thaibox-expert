using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop2.Models
{
    public class Bestelling
    {
        public int bestellingID { get; set; }
        public int totaalPrijs { get; set; }
        public string bestellingStatus { get; set; }
        public bool betaald { get; set; }
        public DateTime bezorgDatum { get; set; }
        public int gebruiker { get; set; }
        public DateTime bestelDatum { get; set; }
    }
}