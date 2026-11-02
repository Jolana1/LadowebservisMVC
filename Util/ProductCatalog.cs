using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LadowebservisMVC.Util
{
    public class ProductInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
    }

    public static class ProductCatalog
    {
        // Current Slovak retail/base prices from the official Zinzino SK shop (2026-09-29).
        // Product 9 is a custom bundle calculated from one unit of each current component.
        // Product 7 is a custom/non-Zinzino item and is therefore kept at 3.00 €.
        // The website's 10% promotional discount is applied by the cart to the selected IDs.
        private static readonly Dictionary<string, ProductInfo> productsById = new Dictionary<string, ProductInfo>
        {
            { "0", new ProductInfo { Id = "0", Name = "BalanceTest", Price = 189.00m, Image = "/Image/BalanceTest.png", Stock = 0 } },
            { "1", new ProductInfo { Id = "1", Name = "BalanceOil+ Orange (300ml)", Price = 61.00m, Image = "/Image/BalanceOil.png", Stock = 2 } },
            { "2", new ProductInfo { Id = "2", Name = "Zinobiotic", Price = 47.00m, Image = "/Image/Zinobiotic2025.png", Stock = 3 } },
            { "3", new ProductInfo { Id = "3", Name = "ZinzinoXtend", Price = 47.00m, Image = "/Image/ZinzinoXtend.png", Stock = 0 } },
            { "4", new ProductInfo { Id = "4", Name = "BalanceOil+ Orange (100ml)", Price = 26.00m, Image = "/Image/BalanceOil orange 100ml.png", Stock = 2 } },
            { "5", new ProductInfo { Id = "5", Name = "Vitamin D Test", Price = 74.00m, Image = "/Image/VitaminDTest.png", Stock = 0 } },
            { "6", new ProductInfo { Id = "6", Name = "Davkovacie pohare", Price = 12.00m, Image = "/Image/Davkovacie pohare.png", Stock = 1 } },
            { "7", new ProductInfo { Id = "7", Name = "Balzam", Price = 3.00m, Image = "/Image/Balzam.png", Stock = 1 } },
            { "8", new ProductInfo { Id = "8", Name = "CollagenBoozt", Price = 80.00m, Image = "/Image/CollagenBoozt.png", Stock = 1 } },
            { "9", new ProductInfo { Id = "9", Name = "BalancePack 4-in-1", Price = 235.00m, Image = "/Image/BalancePack4v1.png", Stock = 1 } },
            { "10", new ProductInfo { Id = "10", Name = "ENERGIZE 24H Cream", Price = 77.00m, Image = "/Image/Energize24hcream.png", Stock = 1 } },
            { "11", new ProductInfo { Id = "11", Name = "Arize Day Cream", Price = 88.00m, Image = "/Image/Arizedaycream.png", Stock = 1 } },
            { "12", new ProductInfo { Id = "12", Name = "Skin Serum, 50 ml", Price = 59.00m, Image = "/Image/SkinSerum50ml.png", Stock = 1 } },
            { "13", new ProductInfo { Id = "13", Name = "ShakeBottle, 500 ml", Price = 10.00m, Image = "/Image/Shakebottle.png", Stock = 1 } },
            



            // IT Services
            { "100", new ProductInfo { Id = "100", Name = "Tvorba webovej stránky", Price = 299.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "101", new ProductInfo { Id = "101", Name = "E-shop na mieru", Price = 599.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "102", new ProductInfo { Id = "102", Name = "Oprava PC/notebooku", Price = 45.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "103", new ProductInfo { Id = "103", Name = "Inštalácia softvéru", Price = 25.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "104", new ProductInfo { Id = "104", Name = "IT poradenstvo (1 hod)", Price = 35.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "105", new ProductInfo { Id = "105", Name = "Hosting (mesačný)", Price = 15.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 }
        }
        };


        public static bool TryGetById(string id, out ProductInfo info)
        {
            info = null;
            if (string.IsNullOrWhiteSpace(id)) return false;
            return productsById.TryGetValue(id, out info);
        }

        public static bool TryGetByName(string name, out ProductInfo info)
        {
            info = null;
            if (string.IsNullOrWhiteSpace(name)) return false;
            foreach (var kv in productsById)
            {
                if (kv.Value.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    info = kv.Value;
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<ProductInfo> GetAll() => productsById.Values;
    }
}
