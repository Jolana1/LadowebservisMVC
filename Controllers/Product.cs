public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; } // URL to the product image

    public int Stock { get; set; } // Number of items in stock

    public string Category { get; set; } // Category of the product

    public Product(int id, string name, decimal price, string description, string imageUrl, int stock, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
        Stock = stock;
        Category = category;
    }
}





