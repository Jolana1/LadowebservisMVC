using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace LadowebservisMVC.Models
{
    public class ZberUdajovModel
    {
        
        public string Url;
        public string Params;
        public string Identifikator { get; set; }

        [Required(ErrorMessage ="Vstup 1 musí byť zadaný")]
        [Display(Name = "Vstup 1")]
        public string Vstup1 { get; set; }

        [Required(ErrorMessage ="Musíte zadať iba číslo napr. v tvare 123")]
        [Display(Name = "Vstup 2")]
        public string Vstup2 { get; set; }

        public NameValueCollection ParamList;


    }
}