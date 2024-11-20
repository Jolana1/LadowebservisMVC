using LadowebservisMVC.Controllers;
using LadowebservisMVC.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LadowebservisMVC.Models
{
    public class OdoslanieRegModel
    {

        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Meno")]
        public string Name { get; set; }

        /// <summary>
        /// Lastname
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Priezvisko")]

        public string Priezvisko { get; set; }

        /// Email
        /// </summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [EmailAddress(ErrorMessage = ModelUtil.invalidEmailErrMessage_Sk)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        /// <summary>

        /// Phone
        /// <summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        /// <summary>

        /// Adress
        /// <summary>

        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Adresa")]

        public string Adresa { get; set; }

        ///City
        ///<summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Mesto alebo obec")]

        public string City { get; set; }

        /// Text
        /// <summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Display(Name = "Text")]
        public string Text { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        public string Password { get; set; }
        /// <summary>
        /// Captcha 
        //</summary>
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [Compare("Captcha", ErrorMessage = "Emaily sa nezhodujú")]
        [Display(Name = "UserName")]
        public string Captcha { get; set; }










    }
}