using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;






namespace LadowebservisMVC.Models
{

    public class TextTemplateModel
    {

        [Required(ErrorMessage = "Meno a Priezvisko musí byť zadané")]
        [Display(Name = "Meno a Priezvisko:")]
        public string Name { get; set; }

        
        [Display(Name = "Email musí byť zadaný: ")]
        [Required(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefón musí byť zadaný")]
        [Display(Name = "Telefón")]

        public int Phone { get; set; }

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


















