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

        // Optional: additional notes from user
        [DataType(DataType.MultilineText)]
        [Display(Name = "Poznámka")]
        public string Note { get; set; }

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