using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop2.Models
{
    public class Product
    {
        public String productNaam { get; set; }
        public String productDetail { get; set; }
        public String productMerk { get; set; }
        public int productID { get; set; }
        public int productPrijs { get; set; }
<<<<<<< HEAD
        public String productSoort { get; set; }
        public String productAfbeelding { get; set; }
=======
        public int productAantal { get; set; }
>>>>>>> origin/master
    }
}