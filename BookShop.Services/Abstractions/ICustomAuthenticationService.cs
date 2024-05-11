using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICustomAuthenticationService
{
    string GenerateToken(ClientLoginVm client);
    string GetClientEmailFromToken(string token);
}
