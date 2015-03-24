using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Webshop2.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Naam { get; set; }

        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Woonadres { get; set; }

        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Woonpostcode { get; set; }

        [Required(ErrorMessage = "Dit veld is verplicht")]
        public String Woonplaats { get; set; }

        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Gebruikersnaam { get; set; }

        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Wachtwoord { get; set; }

        [CompareAttribute("Wachtwoord", ErrorMessage = "Uw wachtwoorden komen niet overeen.")]
        public String BevestigdWachtwoord { get; set; }
        
        [Required(ErrorMessage="Dit veld is verplicht")]
        public String Email { get; set; }

        public DateTime GeboorteDatum { get; set; }

        public Int32 Telefoonnummer { get; set; }

        public String Leveradres { get; set; }

        public String Leverpostcode { get; set; }
    }
}