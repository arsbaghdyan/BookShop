using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface IShopAuthenticationService
{
    string GenerateClientToken(ClientModel client);
}
