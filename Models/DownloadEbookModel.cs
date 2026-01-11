using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LadowebservisMVC.Models
{
    /// <summary>
    /// Model for the E-book Download form (DownloadEbook action)
    /// </summary>
    public class DownloadEbookModel
    {
        [Required(ErrorMessage = "Meno musí byť zadané")]
        [Display(Name = "Meno a Priezvisko")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Meno musí mať 2 až 100 znakov")]
        [RegularExpression(
            @"^[a-zA-ZáäčďéíĺľňóôŕšťúůýžÁÄČĎÉÍĹĽŇÓÔŔŠŤÚŮÝŽ\s\-']{2,}$",
            ErrorMessage = "Meno smie obsahovať len písmená, medzery a pomlčky")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email musí byť zadaný")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        [StringLength(100, ErrorMessage = "Email nesmie mať viac ako 100 znakov")]
        public string Email { get; set; }

        [Display(Name = "Vaša správa")]
        [StringLength(500, ErrorMessage = "Správa nesmie mať viac ako 500 znakov")]
        public string Sprava { get; set; }

        [Required(ErrorMessage = "Bezpečnostný kód musí byť zadaný")]
        [Display(Name = "Bezpečnostný kód")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Bezpečnostný kód nie je platný")]
        public string Captcha { get; set; }

        [Display(Name = "Príloha")]
        public HttpPostedFileBase File { get; set; }
    }
}


















