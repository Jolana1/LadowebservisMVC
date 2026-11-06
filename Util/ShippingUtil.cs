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

        private static readonly Dictionary<string, string> MethodKeys =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "courier", "Shipping:Courier:RatesJson" },
                { "gls-courier", "Shipping:GlsCourier:RatesJson" },
                { "gls-parcelshop", "Shipping:GlsParcelShop:RatesJson" },
                { "gls-box", "Shipping:GlsBox:RatesJson" },
                { "dpd-courier", "Shipping:DpdCourier:RatesJson" },
                { "dpd-pickup", "Shipping:DpdPickup:RatesJson" },
                { "packeta", "Shipping:Zasielkovna:RatesJson" },
                { "packeta-zbox", "Shipping:PacketaZBox:RatesJson" }
            };

        public static decimal GetShippingFee(string shippingMethod, decimal cartTotal)
        {
            string method = (shippingMethod ?? "").Trim().ToLowerInvariant();
            switch (method)
            {
                case "zasielkovna": method = "packeta"; break;
                case "packetazbox":
                case "z-box":
                case "zbox": method = "packeta-zbox"; break;
                case "glscourier": method = "gls-courier"; break;
                case "glsparcelshop":
                case "gls-pickup": method = "gls-parcelshop"; break;
                case "glsbox": method = "gls-box"; break;
                case "dpdcourier":
                case "dpd_kurier": method = "dpd-courier"; break;
                case "dpdpickup":
                case "dpd-point":
                case "dpdpoint": method = "dpd-pickup"; break;
            }
            string key;
            if (!MethodKeys.TryGetValue(method, out key))
                throw new ArgumentException("Nepodporovany sposob dopravy.", "shippingMethod");
            var json = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(json))
                throw new ConfigurationErrorsException("Chyba konfiguracia dopravy: " + key);
            List<Rate> rates;
            try { rates = new JavaScriptSerializer().Deserialize<List<Rate>>(json); }
            catch (Exception ex) { throw new ConfigurationErrorsException("Neplatny cennik dopravy: " + key, ex); }
            if (rates == null || rates.Count == 0)
                throw new ConfigurationErrorsException("Prazdny cennik dopravy: " + key);
            Rate selected = null;
            foreach (var rate in rates)
            {
                if (rate == null || rate.MinTotal < 0 || rate.Price < 0)
                    throw new ConfigurationErrorsException("Neplatna sadzba dopravy: " + key);
                if (rate.MinTotal <= cartTotal && (selected == null || rate.MinTotal > selected.MinTotal))
                    selected = rate;
            }
            if (selected == null)
                throw new ConfigurationErrorsException("Chyba sadzba dopravy pre kosik: " + key);
            return selected.Price;
        }
    }
}
