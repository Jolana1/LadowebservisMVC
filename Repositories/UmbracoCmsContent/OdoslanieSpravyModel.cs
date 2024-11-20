using dufeksoft.lib.Mail;
using dufeksoft.lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UmbracoEshop.lib.Util;
using Mailer = UmbracoEshop.lib.Util.Mailer;

namespace UmbracoEshop.lib.Models
{
    public class OdoslanieSpravyModel
    {
        
        [Required(ErrorMessage = "Priezvisko a meno musí byť zadané")]
        [Display(Name = "Priezvisko a meno")]
        public string Name { get; set; }
        [Email(ErrorMessage = ModelUtil.invalidEmailErrMessage_Sk)]
        [Required(ErrorMessage = "E-mail musí byť zadaný")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Text správy musí byť zadaný")]
        [Display(Name = "Text správy")]
        public string Text { get; set; }

        
        public string Password { get; set; }
        [Display(Name = "Heslo")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Potvrdenie hesla")]
        public bool SendContactRequest()
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam> { };
            paramList.Add(new TextTemplateParam("NAME", this.Name));
            paramList.Add(new TextTemplateParam("EMAIL", this.Email));
            paramList.Add(new TextTemplateParam("TEXT", this.Text));

            // Odoslanie uzivatelovi
            Mailer.SendMailTemplate(
                "Vaša správa",
                TextTemplate.GetTemplateText("ContactSendSuccess_Sk", paramList),
                this.Email, null);

            return true;
        }
    }
}




