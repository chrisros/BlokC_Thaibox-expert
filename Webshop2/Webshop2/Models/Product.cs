using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Webshop2.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Dit veld is verplicht")]
        public String productNaam { get; set; }
        [Required(ErrorMessage = "Dit veld is verplicht")]
        public String productDetail { get; set; }
        public String productMerk { get; set; }
        public int productID { get; set; }
        [Required(ErrorMessage = "Dit veld is verplicht")]
        public int productPrijs { get; set; }
        [Required(ErrorMessage = "Dit veld is verplicht")]
        public String productSoort { get; set; }
        public String productAfbeelding { get; set; }

        public int productAantal { get; set; }

    }
}