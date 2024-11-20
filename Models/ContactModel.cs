using LadowebservisMVC.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LadowebservisMVC.Models.NumberAttribute;



namespace LadowebservisMVC.Controllers
{
    public class ContactModel 
    {
        [Required(ErrorMessage = "Meno a Priezvisko musí byť zadané")]
        [Display(Name = "Meno a Priezvisko:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email musí byť zadaný")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefón musí byť zadaný")]
        [Display(Name = "Telefón")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Text musi byť zadaný")]
        [Display(Name = "Text správy:")]
        public string Sprava { get; set; }

        [Display(Name = "Captcha")]
        [Required(ErrorMessage = "Zopakujte ešte raz emailovú adresu kvôli kontrole")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Captcha { get; set; }
    }
}













