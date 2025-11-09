using LadowebservisMVC.Util;
using System.Collections.Generic;
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

        //[Required(ErrorMessage = ModelUtil.invalidPhoneErrMessage_Sk)]
        //[RegularExpression(ModelUtil.phoneRegex)]
        //[DataType(DataType.PhoneNumber)]
        //[Display(Name = "Phone")]
        //public string Phone { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "File")]
        public HttpPostedFileBase File { get; set; }




        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.Text)]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool TexTemplate()
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>
                {
                    new TextTemplateParam("NAME", this.Name),
                    new TextTemplateParam("EMAIL", this.Email),
                    new TextTemplateParam("TEXT", this.Text),
                   // new TextTemplateParam("PHONE", this.Phone),
                    new TextTemplateParam("PASSWORD", this.Password)
                };

            Mailer mailer = new Mailer();
            // pass null to ensure Mailer still attaches the App_Data/MailAttachment.pdf
            mailer.OdoslanieSpravy(this, null);

            return true;
        }
    }
}

