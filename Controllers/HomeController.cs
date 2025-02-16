using LadowebservisMVC.Controllers.Models;
using LadowebservisMVC.Models;
using LadowebservisMVC.Util;
using System.Web.Mvc;


namespace LadowebservisMVC.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.PageTitle = "Index";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.PageTitle = "O nás";
            return View();
        }

        
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Email == "Name" && model.Password == "Password")
                {
                    return RedirectToAction("Member", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nesprávne meno alebo heslo");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Member(MemberModel model)
        {
            ViewBag.PageTitle = "Member";
            if (ModelState.IsValid)
            {
                if (model.Meno == "Name" && model.Heslo == "Heslo")
                {
                    return RedirectToAction("Member", "Home");
                }

            }
            return View(model);
        }

        public ActionResult MemberInfo()
        {
            ViewBag.PageTitle = "Member";
            return View();
        }

        public ActionResult Kontakt()
        {
            ViewBag.PageTitle = "Kontakt";
            ContactModel_Sk model = new ContactModel_Sk();
            return View(model);
        }

        [HttpPost]
        public ActionResult OdoslanieSpravy(ContactModel_Sk model)
        {
            ViewBag.PageTitle = "OdoslanieSpravy";
            if (ModelState.IsValid)
            {
                Mailer mailer = new Mailer();
                mailer.OdoslanieSpravy(model);

                return View();
            }
            return View("Kontakt", model);
        }

        public ActionResult Registracia()
        {
            ViewBag.PageTitle = "Registracia";
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult OdoslanieReg(RegisterModel model)
        {
            ViewBag.PageTitle = "OdoslanieReg";
            if (ModelState.IsValid)
            {
                Mailer mailer = new Mailer();
                mailer.OdoslanieEmailu(model);

                return View();
            }
            return View("Registracia", model);
        }

        public ActionResult Hudba()
        {
            ViewBag.PageTitle = "Hudba";
            return View();
        }

       

        public ActionResult Objednavky()
        {
            ViewBag.PageTitle = "Objednavky";
            
            return View();
        }

        public ActionResult Produkty()
        {
            ViewBag.PageTitle = "Produkty";
            OrderModel model = new OrderModel();
            return View(model);
           
        }

        public ActionResult Kosik()
        {
            ViewBag.PageTitle = "Kosik";
            return View();
        }


    }
}



















