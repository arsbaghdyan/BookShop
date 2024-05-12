namespace BookShop.Services.Models.CartItemModels;

public class ProductAddModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public string Details { get; set; }
    public int Count { get; set; }
}
