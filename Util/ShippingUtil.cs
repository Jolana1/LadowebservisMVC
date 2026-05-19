using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;

namespace LadowebservisMVC.Util
{
    public static class ShippingUtil
    {
        private sealed class Rate
        {
            public decimal MinTotal { get; set; }
            public decimal Price { get; set; }
        }

        public static decimal GetShippingFee(string shippingMethod, decimal cartTotal)
        {
            if (string.IsNullOrWhiteSpace(shippingMethod)) return 0m;

            var method = shippingMethod.Trim().ToLowerInvariant();

            if (method == "zasielkovna" || method == "packeta")
            {
                return GetTieredFee("Shipping:Zasielkovna:RatesJson", cartTotal);
            }

            if (method == "courier")
            {
                return GetTieredFee("Shipping:Courier:RatesJson", cartTotal);
            }

            if (method == "dpd-courier" || method == "dpdcourier" || method == "dpd_kurier")
            {
                return GetTieredFee("Shipping:DpdCourier:RatesJson", cartTotal);
            }

            if (method == "dpd-pickup" || method == "dpdpickup" || method == "dpd-point" || method == "dpdpoint")
            {
                return GetTieredFee("Shipping:DpdPickup:RatesJson", cartTotal);
            }

            return 0m;
        }

        private static decimal GetTieredFee(string appSettingKey, decimal cartTotal)
        {
            var json = ConfigurationManager.AppSettings[appSettingKey];
            if (string.IsNullOrWhiteSpace(json)) return 0m;

            try
            {
                var js = new JavaScriptSerializer();
                var rates = js.Deserialize<List<Rate>>(json) ?? new List<Rate>();
                if (rates.Count == 0) return 0m;

                // pick the highest MinTotal <= cartTotal
                Rate selected = null;
                for (var i = 0; i < rates.Count; i++)
                {
                    var r = rates[i];
                    if (r == null) continue;
                    if (r.MinTotal <= cartTotal)
                    {
                        if (selected == null || r.MinTotal > selected.MinTotal)
                        {
                            selected = r;
                        }
                    }
                }

                return selected != null ? selected.Price : 0m;
            }
            catch
            {
                return 0m;
            }
        }
    }
}
