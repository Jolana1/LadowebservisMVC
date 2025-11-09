using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LadowebservisMVC.Controllers
{

    public class Controller 
    {
        // GET: Controller
        public ActionResult Member()
        {
            ViewBag.PageTitle = "Member";
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Email == "Name" && model.Heslo == "Password")
                {
                    return RedirectToAction("Member", "Controller");
                }
                else
                {
                    ModelState.AddModelError("Skontrolujte zadane udaje", "");
                    return RedirectToAction("Login", "Controller");
                }
            }
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.PageTitle = "Kontakt";
            ContactModel model = new ContactModel();
            return View(model);
        }
        public ActionResult OdoslanieSpravy(ContactModel model)
        {
            ViewBag.PageTitle = "OdoslanieSpravy";
            if (!ModelState.IsValid)
            {
                return View("Contact", model);
            }
            return View();
        }
        public ActionResult Ponukasluzby()
        {
            ViewBag.PageTitle = "ponuka-služby";
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.PageTitle = "index";
            return View();
        }
        public ActionResult Memberinfo()
        {
            ViewBag.PageTitle = "memberinfo";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.PageTitle = "Kontakt";
            OdoslanieSpravyModel model = new OdoslanieSpravyModel();
            return View(model);
        }
        public ActionResult OdoslanieSpravy(OdoslanieSpravyModel model)
        {
            ViewBag.PageTitle = "Odoslanie Spravy";
            if (!ModelState.IsValid)
            {
                return View("Kontakt", model);
            }
            return View(model);
        }
        public ActionResult Registracia()
        {
            ViewBag.PageTitle = "Registracia";
            RegisterModel model = new RegisterModel();
            return View(model);
        }
        public ActionResult Kosik()
        {
            ViewBag.PageTitle = "kosik";

            return View(model);
        }
    }



