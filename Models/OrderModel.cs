using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Xml.Linq;
using System.Xml;



namespace LadowebservisMVC.Models
{
    public class OrderModel 
{
        public const string EmptyImgUrl = "/Image/3kroky 2.png";

        [Display(Name = "Zobrazovať")]
        public bool ProductIsVisible { get; set; }
        public string ProductIsVisibleText { get; set; }

        [Required(ErrorMessage = "Výrobca musí byť zadaný")]
        [Display(Name = "Výrobca")]
        public string ProducerCollectionKey { get; set; }
        public Guid ProducerKey { get; set; }
        public string ProducerName { get; set; }



        [Required(ErrorMessage = "Kód výrobku musí byť zadaný")]
        [Display(Name = "Kód výrobku")]
        public string KodVyrobku { get; set; }

        [Required(ErrorMessage = "Názov výrobku musí byť zadaný")]
        [Display(Name = "Názov výrobku")]
        public string NazovVyrobku { get; set; }

        [Required(ErrorMessage = "Cena výrobku musí byť zadaná")]
        [Display(Name = "Cena výrobku")]
        public string CenaVyrobku { get; set; }

        [AllowHtml]
        [Display(Name = "Popis výrobku")]
        public string PopisVyrobku { get; set; }
        [AllowHtml]
        [Display(Name = "Poradie")]
        public int ProductOrder { get; set; }
        
        [Display(Name = "Obrázok")]
        public string ProductImg { get; set; }

        public string AdminImgUrl
        {
            get
            {
                return string.IsNullOrEmpty(this.ProductImg) ? OrderModel.EmptyImgUrl : this.ProductImg;
                
            }
        }

        public string CenaVyrobkuAMena()
        {
            return this.CenaVyrobku + " €";
        }
    }
}






