using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Webshop2.Models
{
    public class Categorie
    {
        public int categorieID { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht")]
        public string categorieNaam { get; set; }
    }
}