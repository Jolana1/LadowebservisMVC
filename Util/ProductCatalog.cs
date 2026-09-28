using System.Collections.Generic;

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
        // Static in-memory catalog. Replace with DB lookup in production.
        private static readonly Dictionary<string, ProductInfo> productsById = new Dictionary<string, ProductInfo>
        {
            { "0", new ProductInfo { Id = "0", Name = "BalanceTest", Price = 175.50m, Image = "/Image/BalanceTest.png", Stock = 0 } },
            { "1", new ProductInfo { Id = "1", Name = "BalanceOil+ Orange (300ml)", Price = 54.90m, Image = "/Image/BalanceOil.png", Stock = 2 } },
            { "2", new ProductInfo { Id = "2", Name = "Zinobiotic", Price = 36.00m, Image = "/Image/Zinobiotic2025.png", Stock = 3 } },
            { "3", new ProductInfo { Id = "3", Name = "ZinzinoXtend", Price = 39.60m, Image = "/Image/ZinzinoXtend.png", Stock = 0 } },
            { "4", new ProductInfo { Id = "4", Name = "BalanceOil+ Orange (100ml)", Price = 18.00m, Image = "/Image/BalanceOil orange 100ml.png", Stock = 2 } },
            { "5", new ProductInfo { Id = "5", Name = "Vitamin D Test", Price = 58.50m, Image = "/Image/VitaminDTest.png", Stock = 0 } },
            { "6", new ProductInfo { Id = "6", Name = "Davkovacie pohare", Price = 15.00m, Image = "/Image/Davkovacie pohare.png", Stock = 1 } },
            { "7", new ProductInfo { Id = "7", Name = "Balzam", Price = 3.00m, Image = "/Image/Balzam.png", Stock = 1 } },
            { "8", new ProductInfo { Id = "8", Name = "CollagenBoozt", Price = 58.50m, Image = "/Image/CollagenBoozt.png", Stock = 1 } },
            { "9", new ProductInfo { Id = "9", Name = "BalancePack 4-in-1", Price = 173.70m, Image = "/Image/BalancePack4v1.png", Stock = 1 } },

            
            // IT Services
            { "100", new ProductInfo { Id = "100", Name = "Tvorba webovej stránky", Price = 299.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "101", new ProductInfo { Id = "101", Name = "E-shop na mieru", Price = 599.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "102", new ProductInfo { Id = "102", Name = "Oprava PC/notebooku", Price = 45.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "103", new ProductInfo { Id = "103", Name = "Inštalácia softvéru", Price = 25.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "104", new ProductInfo { Id = "104", Name = "IT poradenstvo (1 hod)", Price = 35.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } },
            { "105", new ProductInfo { Id = "105", Name = "Hosting (mesačný)", Price = 15.00m, Image = "/Image/LWSGEN Logo.png", Stock = 999 } }

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