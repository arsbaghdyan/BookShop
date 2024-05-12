namespace BookShop.Services.Models.CartItemModels;

public class CartItemUpdateModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long CartId { get; set; }
    public long Count { get; set; }
    public decimal Price { get; set; }
}
