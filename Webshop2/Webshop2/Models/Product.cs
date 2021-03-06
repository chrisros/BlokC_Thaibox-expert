﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        public double productPrijs { get; set; }
        public string productPrijsString { get; set; }
        [Required(ErrorMessage = "Dit veld is verplicht")]
        public int productCat { get; set; }
        public int categorieNaam { get; set; }
        //public String productAfbeelding { get; set; }

        public string ImageData { get; set; }
        public string productAfbeelding { get; set; }
        public string productMateriaal { get; set; }
        public int uitvoeringVoorraad { get; set; }
        public string uitvoeringKleur { get; set; }
        public string productGewicht { get; set; }
        public string productAuteur { get; set; }
        public string productUitgever { get; set; }
        public string productGeslacht { get; set; }
        public int productAantal { get; set; }
        public string productMaat { get; set; }
        public string productKleur { get; set; }
        public int productUitvoeringID { get; set; }
        public string uitvoeringMaat { get; set; }
        public int uitvoeringID { get; set; }
    }
}