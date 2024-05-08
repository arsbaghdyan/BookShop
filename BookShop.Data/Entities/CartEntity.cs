using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class CartEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public ClientEntity? ClientEntity { get; set; }
    public List<CartItemEntity> CartItems { get; set; } = new();
}