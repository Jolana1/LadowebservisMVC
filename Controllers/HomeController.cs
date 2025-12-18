using LadowebservisMVC.Controllers.Models;
using LadowebservisMVC.Models;
using LadowebservisMVC.Util;
using System;
using System.Net.Mail;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Web.Script.Serialization; // fixed using
using System.Linq;
using System.Collections.Generic;

namespace LadowebservisMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Zdravie()
        {
            ViewBag.PageTitle = "zdravie";
            return View();
        }

        public ActionResult ITservis()
        {
            ViewBag.PageTitle = "itservis";
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            // Only attempt login when model is valid
            if (ModelState.IsValid && model != null)
            {
                if (model.Email == "Name" && model.Password == "Password")
                {
                    // create MemberInfoModel from LoginModel values (map by property name) and save to session
                    try
                    {
                        var memberInfo = new MemberInfoModel();
                        var fromType = model.GetType();
                        var toType = memberInfo.GetType();
                        foreach (var p in fromType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                        {
                            try
                            {
                                var dest = toType.GetProperty(p.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                if (dest != null && dest.CanWrite)
                                {
                                    dest.SetValue(memberInfo, p.GetValue(model, null));
                                }
                            }
                            catch { }
                        }
                        Session["MemberInfo"] = memberInfo;
                    }
                    catch { }

                    // redirect to member information page on successful login
                    return RedirectToAction("MemberInfo", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Neplatné prihlasovacie údaje.");
                }
            }
            return View(model);
        }

        // GET: Member (show member form)
        public ActionResult Member()
        {
            ViewBag.PageTitle = "Member";
            return View(new MemberModel());
        }

        [HttpPost]
        public ActionResult Member(MemberModel model)
        {
            ViewBag.PageTitle = "Member";
            if (ModelState.IsValid && model != null)
            {
                if (model.Meno == "Name" && model.Heslo == "Heslo")
                {
                    // redirect to member info on success
                    return RedirectToAction("MemberInfo", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Neplatné údaje člena.");
                }
            }
            return View(model);
        }

        public ActionResult MemberInfo()
        {
            ViewBag.PageTitle = "Member";
            // Read MemberInfoModel from session if available
            var mi = Session["MemberInfo"] as MemberInfoModel;
            if (mi == null) mi = new MemberInfoModel();
            return View(mi);
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

        // Show registration confirmation page
        [HttpGet]
        public ActionResult OdoslanieReg()
        {
            ViewBag.PageTitle = "OdoslanieReg";
            return View();
        }

        [HttpPost]
        public ActionResult OdoslanieReg(RegisterModel model)
        {
            ViewBag.PageTitle = "OdoslanieReg";
            if (ModelState.IsValid)
            {
                var mailer = new Mailer();
                mailer.OdoslanieEmailu(model);

                // allow access to ordering after successful registration
                TempData["CanOrder"] = true;

                // Redirect to the registration confirmation page
                return RedirectToAction("OdoslanieReg", "Home");
            }

            // On validation errors, re-render registration with validation messages
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

            // Simple search by name or id via query string
            try
            {
                var q = (Request?["q"] ?? string.Empty).Trim();
                var id = (Request?["id"] ?? string.Empty).Trim();
                var all = ProductCatalog.GetAll() ?? new List<ProductInfo>();

                IEnumerable<ProductInfo> results = all;
                if (!string.IsNullOrEmpty(id))
                {
                    results = results.Where(p => p != null && string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(q))
                {
                    results = results.Where(p => p != null && (p.Name ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                ViewBag.Products = results.ToList();
            }
            catch { ViewBag.Products = new List<ProductInfo>(); }

            return View(model);

        }

        public ActionResult Kosik()
        {
            ViewBag.PageTitle = "Kosik";
            return View();
        }

        public ActionResult Favorites()
        {
            ViewBag.PageTitle = "Obľúbené produkty";
            return View();
        }

        // Place order (bank transfer)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(PlaceOrderModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Produkty");
            }

            // parse cart JSON into Items
            if (string.IsNullOrWhiteSpace(model.CartJson))
            {
                ModelState.AddModelError("CartJson", "Nákupný košík je prázdny.");
            }
            else
            {
                try
                {
                    var js = new JavaScriptSerializer();
                    // expect CartJson to be an array of items matching OrderItem
                    model.Items = js.Deserialize<List<OrderItem>>(model.CartJson) ?? new List<OrderItem>();
                }
                catch
                {
                    model.Items = new List<OrderItem>();
                    ModelState.AddModelError("CartJson", "Chyba pri spracovaní košíka.");
                }
            }

            // Server-side validation: compare prices and stock with ProductCatalog
            var catalogErrors = new List<string>();
            foreach (var item in model.Items)
            {
                if (ProductCatalog.TryGetById(item.Id, out var info))
                {
                    // check stock
                    if (item.Quantity <= 0 || item.Quantity > info.Stock)
                    {
                        catalogErrors.Add($"Nesprávne množstvo pre produkt {info.Name}.");
                    }

                    // check price - if client-supplied UnitPrice differs, replace with catalog price and note discrepancy
                    if (item.UnitPrice != info.Price)
                    {
                        // replace client price with trusted price
                        item.UnitPrice = info.Price;
                        catalogErrors.Add($"Cena produktu {info.Name} bola upravená podľa katalógu.");
                    }
                }
                else
                {
                    catalogErrors.Add($"Produkt s ID '{item.Id}' neexistuje.");
                }
            }

            if (catalogErrors.Any())
            {
                foreach (var e in catalogErrors) ModelState.AddModelError("CartJson", e);
            }

            // compute total
            var total = model.Items?.Sum(i => i.LineTotal) ?? 0m;

            if (!ModelState.IsValid)
            {
                // return to cart view with errors (keep simple: redirect to Kosik)
                return RedirectToAction("Kosik");
            }

            // Here you could persist the order, reduce stock, send confirmation email, etc.

            ViewBag.Items = model.Items;
            ViewBag.Total = total;
            ViewBag.PaymentMethod = "bank";

            return View("OrderPlaced");
        }

        // Start Stripe checkout - parse cart and forward to payment gateway integration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartStripeCheckout(PlaceOrderModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Produkty");
            }

            try
            {
                var js = new JavaScriptSerializer();
                model.Items = js.Deserialize<List<OrderItem>>(model.CartJson) ?? new List<OrderItem>();
            }
            catch
            {
                model.Items = new List<OrderItem>();
            }

            var total = model.Items?.Sum(i => i.LineTotal) ?? 0m;

            if (model.Items == null || model.Items.Count == 0)
            {
                return RedirectToAction("Kosik");
            }

            // TODO: integrate with Stripe API. For now show OrderPlaced summary with payment method stripe
            ViewBag.Items = model.Items;
            ViewBag.Total = total;
            ViewBag.PaymentMethod = "stripe";

            return View("OrderPlaced");
        }

        // Start PayPal checkout - parse cart and forward to PayPal integration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartPayPalCheckout(PlaceOrderModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Produkty");
            }

            try
            {
                var js = new JavaScriptSerializer();
                model.Items = js.Deserialize<List<OrderItem>>(model.CartJson) ?? new List<OrderItem>();
            }
            catch
            {
                model.Items = new List<OrderItem>();
            }

            var total = model.Items?.Sum(i => i.LineTotal) ?? 0m;

            if (model.Items == null || model.Items.Count == 0)
            {
                return RedirectToAction("Kosik");
            }

            // TODO: integrate with PayPal API. For now show OrderPlaced summary with payment method paypal
            ViewBag.Items = model.Items;
            ViewBag.Total = total;
            ViewBag.PaymentMethod = "paypal";

            return View("OrderPlaced");
        }

        public ActionResult ReturnPolicy()
        {
            ViewBag.PageTitle = "ReturnPolicy";
            return View();
        }
    }
}


































































































































































































































































































































































































































































































































