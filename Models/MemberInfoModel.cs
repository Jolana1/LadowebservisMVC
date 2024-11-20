using System.ComponentModel.DataAnnotations;

namespace LadowebservisMVC.Models
{
    public class MemberInfoModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Nezadali ste platnú emailovú adresu")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Heslo musí byť zadané")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}




