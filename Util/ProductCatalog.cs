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
            { "0", new ProductInfo { Id = "0", Name = "BalanceTest", Price = 195.00m, Image = "/Image/BalanceTest.png", Stock = 10 } },
            { "1", new ProductInfo { Id = "1", Name = "BalanceOil+ Orange (300ml) ", Price = 55.00m, Image = "/Image/BalanceOil.png", Stock = 50 } },
            { "2", new ProductInfo { Id = "2", Name = "Zinobiotic", Price = 33.00m, Image = "/Image/Zinobiotic2025.png", Stock = 30 } },
            { "3", new ProductInfo { Id = "3", Name = "ZinzinoXtend", Price = 44.00m, Image = "/Image/ZinzinoXtend.png", Stock = 25 } },
            // Additional products - add images under /Image or adjust paths as needed
            { "4", new ProductInfo { Id = "4", Name = "BalanceOil+ Orange (100ml)", Price = 25.00m, Image = "/Image/BalanceOil orange 100ml.png", Stock = 20 } },
            { "5", new ProductInfo { Id = "5", Name = "Vitamin D Test", Price = 65.00m, Image = "/Image/VitaminDTest.png", Stock = 15 } }
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