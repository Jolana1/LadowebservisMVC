using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LadowebservisMVC.Models
{
    public class Hudba
    {
        public int Id { get; set; }
        public string Nazov { get; set; }
        public string Interpret { get; set; }
        public string Album { get; set; }
        public string Zaner { get; set; }
        public string Rok { get; set; }
        public string Url { get; set; }

    }
}