namespace BookShop.Services.Models.CartItemModels;

public class ProductModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public int Count { get; set; }
}
