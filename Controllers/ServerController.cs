using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Data.Entity; // Add this namespace for Entity Framework
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

            StripeConfiguration.ApiKey = "pk_live_51P80kcHrPMzQ1ua8JUHSe4iUQ9sLHonQMmFzwyRKnq2xTpB6mhuJVc4OdBKa04BJzpsjjliSrBoNnftkBxwntFF300mePdWSx3";

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
                SuccessUrl = Url.Action("Success", "Server", null, Request.Url.Scheme),
                CancelUrl = Url.Action("Cancel", "Server", null, Request.Url.Scheme),
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Json(new { success = true, sessionId = session.Id });
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}