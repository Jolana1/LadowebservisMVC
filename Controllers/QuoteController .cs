using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LadowebservisMVC.lib.Models;
using LadowebservisMVC.lib.Repositories;

namespace LadowebservisMVC.lib.Controllers
{
    [PluginController("LadowebservisMVC")]
    public class QuoteController : _BaseController
    {
        public ActionResult Basket()
        {
            BasketModel model = new BasketModel(this.CurrentSessionId);

            return View(model);
        }
    }
}