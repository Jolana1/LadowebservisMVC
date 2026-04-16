using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Data.Entity; // Add this namespace for Entity Framework
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using LadowebservisMVC.Util;
//using Umbraco.Web.Mvc;
//using LadowebservisMVC.lib.Models; // Adjust the namespace according to your project structure
//using LadowebservisMVC.lib.Repositories; // Adjust the namespace according to your project structure
//using LadowebservisMVC.lib.Models.Paging; // Adjust the namespace according to your project structure
//using LadowebservisMVC.lib.Controllers; // Adjust the namespace according to your project structure
//using LadowebservisMVC.lib.Repositories.VyrobokRepository; // Adjust the namespace according to your project structure


namespace LadowebservisMVC.Controllers
{
    //namespace UmbracoEshop.lib.Controllers
    //{
    //    [PluginController("UmbracoEshop")]
    //    public class PublicProductController : _BaseController
    //    {
    //        public ActionResult GetRecords(int page = 1, string sort = "NazovVyrobku", string sortDir = "ASC")
    //        {
    //            try
    //            {
    //                return GetRecordsView(page, sort, sortDir);
    //            }
    //            catch
    //            {

    //                return GetRecordsView(page, sort, sortDir);
    //            }
    //        }

    //        ActionResult GetRecordsView(int page, string sort, string sortDir)
    //        {


    //            VyrobokRepository repository = new VyrobokRepository();
    //            VyrobokPagingListModel model = VyrobokPagingListModel.CreateCopyFrom(
    //                repository.GetPage(page, _PagingModel.AllItemsPerPage/*DefaultItemsPerPage*/, sort, sortDir)
    //                );

    //            model.SessionId = this.CurrentSessionId;

    //            return View(model);
    //        }
    //    }
    //}
   

    public class ApplicationDbContext : DbContext // Define the missing ApplicationDbContext class
    {
        public DbSet<Product> Products { get; set; }
    }

    public class ServerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<Product> Cart
        {
            get
            {
                if (Session["Cart"] == null)
                    Session["Cart"] = new List<Product>();
                return (List<Product>)Session["Cart"];
            }
            set
            {
                Session["Cart"] = value;
            }
        }

        [HttpGet]
        public ActionResult GetCart()
        {
            return Json(Cart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found." });

            var cart = Cart;
            cart.Add(product);
            Cart = cart;
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            Cart.RemoveAll(p => p.Id == productId);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult BuyProduct(int productId)
        {
            var product = Cart.Find(p => p.Id == productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found in cart." });

            StripeConfiguration.ApiKey = "pk_live_51P80kcHrPMzQ1ua8JUHSe4iUQ9sLHonQMmFzwyRKnq2xTpB6mhuJVc4OdBKa04BJzpsjjliSrBoNnftkBxwntFF300mePdWSx3";
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(product.Price * 100),
                Currency = "eur",
                PaymentMethodTypes = new List<string> { "card" },
                Description = $"Purchase of {product.Name}"
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Json(new
            {
                success = true,
                clientSecret = paymentIntent.ClientSecret
            });
        }

        [HttpPost]
        public ActionResult CreateCheckoutSession(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found." });

            var stripeSecret = ConfigurationManager.AppSettings["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = stripeSecret;

            if (string.IsNullOrWhiteSpace(stripeSecret) || stripeSecret.StartsWith("pk_", StringComparison.OrdinalIgnoreCase) || stripeSecret.IndexOf("YOUR_SECRET", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return Json(new { success = false, message = "Stripe SecretKey is not configured." });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(product.Price * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Server", null, Request.Url.Scheme) + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = Url.Action("Cancel", "Server", null, Request.Url.Scheme),
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Json(new { success = true, sessionId = session.Id });
        }

        public ActionResult Success()
        {
            // Payment confirmation email is sent by Stripe webhook (more reliable).
            ViewBag.SessionId = Request?["session_id"];
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StripeWebhook()
        {
            var json = string.Empty;
            try
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    json = reader.ReadToEnd();
                }

                var stripeSignature = Request.Headers["Stripe-Signature"];
                var webhookSecret = ConfigurationManager.AppSettings["Stripe:WebhookSecret"]; 
                var stripeSecret = ConfigurationManager.AppSettings["Stripe:SecretKey"]; 

                if (string.IsNullOrWhiteSpace(webhookSecret) || string.IsNullOrWhiteSpace(stripeSignature))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                StripeConfiguration.ApiKey = stripeSecret;
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

                if (stripeEvent == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (string.Equals(stripeEvent.Type, "checkout.session.completed", StringComparison.OrdinalIgnoreCase))
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null)
                    {
                        try
                        {
                            var sessionService = new SessionService();
                            var full = sessionService.Get(session.Id, new SessionGetOptions
                            {
                                Expand = new List<string> { "line_items" }
                            });

                            var email = full?.CustomerDetails?.Email ?? session.CustomerDetails?.Email;
                            var name = full?.CustomerDetails?.Name ?? session.CustomerDetails?.Name;

                            if (!string.IsNullOrWhiteSpace(email) && string.Equals(full?.PaymentStatus, "paid", StringComparison.OrdinalIgnoreCase))
                            {
                                var mailer = new Mailer();
                                mailer.SendPaymentConfirmationEmail(full, email, name);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError($"Stripe webhook processing error: {ex}");
                        }
                    }
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Stripe webhook error: {ex}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}