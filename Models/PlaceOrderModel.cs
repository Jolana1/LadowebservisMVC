using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LadowebservisMVC.Controllers.Models
{
    public class PlaceOrderModel
    {
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.Text)]
        [Display(Name = "Meno")]
        public string Name { get; set; }

        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Neplatný formát emailu.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // Serialized cart JSON from client (contains items, quantities, etc.)
        [Required(ErrorMessage = ModelUtil.requiredErrMessage_Sk)]
        [DataType(DataType.Text)]
        [Display(Name = "CartJson")]
        public string CartJson { get; set; }

        // Optional: payment method selected by the user ("bank", "stripe", "paypal")
        [DataType(DataType.Text)]
        [Display(Name = "PaymentMethod")]
        public string PaymentMethod { get; set; }

        // Shipping method selected by the user (e.g., "zasielkovna")
        [DataType(DataType.Text)]
        [Display(Name = "ShippingMethod")]
        public string ShippingMethod { get; set; }

        // Shipping address (used for delivery-to-address methods, e.g., "courier")
        [DataType(DataType.Text)]
        [Display(Name = "Ulica a è.")]
        public string ShippingAddressLine1 { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Doplòujúce údaje")]
        public string ShippingAddressLine2 { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mesto")]
        public string ShippingCity { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "PSÈ")]
        public string ShippingZip { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Krajina")]
        public string ShippingCountry { get; set; }

        // Selected pickup point (provider-specific code/id)
        [DataType(DataType.Text)]
        [Display(Name = "ZasielkovnaPickupPoint")]
        public string ZasielkovnaPickupPoint { get; set; }

        // Selected pickup point name/address shown to the user
        [DataType(DataType.Text)]
        [Display(Name = "ZasielkovnaPickupPointName")]
        public string ZasielkovnaPickupPointName { get; set; }

        // Optional raw JSON details of pickup point (for future debugging / fulfillment)
        [DataType(DataType.Text)]
        [Display(Name = "ZasielkovnaPickupPointJson")]
        public string ZasielkovnaPickupPointJson { get; set; }

        // Optional: additional notes from user
        [DataType(DataType.MultilineText)]
        [Display(Name = "Poznámka")]
        public string Note { get; set; }

        // Full pickup point details JSON (id, name, address, city, zip, etc.) from widget
        [DataType(DataType.Text)]
        [Display(Name = "PickupPointDetailsJson")]
        public string PickupPointDetailsJson { get; set; }

        // Helper: parsed cart (not bound by default, can be populated server-side)
        public List<OrderItem> Items { get; set; }
    }

    // Simple order item used for server-side parsing of CartJson
    public class OrderItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get { return UnitPrice * Quantity; } }
    }
}