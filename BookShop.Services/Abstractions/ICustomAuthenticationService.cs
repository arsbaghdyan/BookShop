using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface ICustomAuthenticationService
{
    string GenerateToken(ClientEntity clientEntity);
}
