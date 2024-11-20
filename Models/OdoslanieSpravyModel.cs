using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;









namespace LadowebservisMVC.Models
{

    public class OdoslanieSpravyModel
    {

        [Required(ErrorMessage = "Meno a Priezvisko musí byť zadané")]
        [Display(Name = "Meno a Priezvisko:")]
        public string Meno { get; set; }

        
        
        [Required(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefón musí byť zadaný")]
        [Display(Name = "Telefón")]
        public int Telefon { get; set; }

        [Required(ErrorMessage = "Text musi byť zadaný")]
        [Display(Name = "Text správy:")]
        public string Sprava { get; set; }

        [Display(Name = "Captcha")]

        [Required(ErrorMessage = "Emaily sa nezhodujú")]
        [Compare("Email", ErrorMessage = "Emaily sa nezhodujú")]


        public string Captcha { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Heslo musí byť zadané")]
        [DataType(DataType.Password)]


        public string Password { get; set; }

        [Display(Name = "Adresa")]
        [Required(ErrorMessage = "Psč musí byť zadané")]
        [DataType(DataType.PostalCode)]


        public string Adresa { get; set; }






    }

}


















