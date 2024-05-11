using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.ClientModels;

public class ClientDeleteModel : IIdentifiable
{
    public long Id { get; set; }
}
