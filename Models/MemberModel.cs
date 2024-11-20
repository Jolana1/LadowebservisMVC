using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LadowebservisMVC.Models
{
    public class MemberModel
    {
        [Required(ErrorMessage = "Meno a Priezvisko musí byť zadané")]
        [Display(Name = "Meno a Priezvisko:")]
        public string Meno { get; set; }

        [Required(ErrorMessage = "Email musí byť zadaný:")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefón musí byť zadaný")]
        [Display(Name = "Telefón")]
        public int Telefon { get; set; }

        [Required(ErrorMessage = "Adresa musí byť zadaná")]
        [Display(Name = "Adresa")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Mesto musí byť zadané")]
        [Display(Name = "Mesto")]
        public string Mesto { get; set; }

        [Required(ErrorMessage = "PSČ musí byť zadané")]
        [Display(Name = "PSČ")]
        public int PSC { get; set; }

        [Required(ErrorMessage = "Heslo musí byť zadané")]
        [Display(Name = "Heslo")]

        public string Heslo { get; set; }

        [Required(ErrorMessage = "Emaily sa nezhodujú")]
        [Display(Name = "Zopakujte email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        
        public string Captcha { get; set; }

        
      
    }
}