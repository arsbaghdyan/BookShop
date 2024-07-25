namespace BookShop.Data.Entities;

public class OrderProduct
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public OrderEntity? Order { get; set; }
}
