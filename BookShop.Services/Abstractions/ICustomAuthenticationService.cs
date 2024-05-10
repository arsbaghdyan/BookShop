using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface ICustomAuthenticationService
{
    string GenerateToken(ClientEntity clientEntity);
    Task<ClientEntity> AuthenticateAsync(string email, string password);
    string GetClientEmailFromToken();
}
