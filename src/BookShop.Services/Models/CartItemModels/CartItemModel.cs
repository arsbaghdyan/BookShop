namespace BookShop.Services.Models.CartItemModels;

public class CartItemModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }
}
