using System.ComponentModel.DataAnnotations;




namespace LadowebservisMVC.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Meno musí byť zadané")]
        [Display(Name = "Meno")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Priezvisko musí byť zadané")]
        [Display(Name = "Priezvisko")]

        public string Priezvisko { get; set; }



        [Required(ErrorMessage = "Email musí byť zadaný:")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Emaily sa nezhodujú")]
        [Display(Name = "Zopakujte email")]
        [Compare("Captcha", ErrorMessage = "Emaily sa nezhodujú")]
        public string Captcha { get; set; }

        [Required(ErrorMessage = "Telefón musí byť zadaný")]
        [Display(Name = "Telefón")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "Zadajte názov ulice a číslo")]
        [Display(Name = "Adresa")]
        

        public string Adresa { get; set; }

        //[Required(ErrorMessage = "PSČ musí byť zadané")]
        //[Display(Name = "PSČ")]
        //[DataType(DataType.PostalCode)]
        //public int PSC { get; set; }

        [Required(ErrorMessage = "Mesto  musí byť zadané")]
        [Display(Name = "Mesto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Krátka správa musí byť zadaná")]
        [Display(Name = "Váš text")]
        public string Text { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Heslo musí byť zadané")]
        [DataType(DataType.Password)]

        public string Password { get; set; }




    }
}




























