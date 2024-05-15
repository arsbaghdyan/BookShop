using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface ICustomAuthenticationService
{
    string GenerateToken(ClientModel client);
}
