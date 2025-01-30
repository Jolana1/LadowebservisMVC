using LadowebservisMVC.Models;

using System.Web.Mvc;






namespace LadowebservisMVC.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member

        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Email == "UserName" && model.Heslo == "Password")
                {
                    return RedirectToAction("Member", "Home");
                }
                
            }
            return View(model);
                }
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Registration", "Home");

            }
            //else
            //{
                
            //    ModelState.AddModelError("", "Nesprávne meno alebo heslo");
            //    return RedirectToAction("Member", "Home");

            //}



        }
    }
}







