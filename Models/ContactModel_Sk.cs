using LadowebservisMVC.Util;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LadowebservisMVC.Controllers.Models
{
    public class ContactModel_Sk
    {
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Neplatný formát emailu.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "File")]
        public HttpPostedFileBase File { get; set; }

        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.Text)]
        [Display(Name = "Text")]
        public string Text { get; set; }

        // Zachované pre kompatibilitu so starším kódom; NEPOUŽÍVA sa ako honeypot.
        // Honeypot website_check sa číta iba z Request.Form v HomeController.
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string CartJson { get; set; }

        // Legacy wrapper: reálne odoslanie vykoná Mailer a pri chybe vyhodí výnimku.
        public bool TexTemplate()
        {
            var mailer = new Mailer();
            mailer.OdoslanieKontaktSpravy(this);
            return true;
        }
    }
}
