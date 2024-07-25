namespace BookShop.Services.Models.ProductModels;

public class ProductUpdateModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public int Count { get; set; }
}
