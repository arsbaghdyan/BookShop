using BookShop.Data.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace BookShop.Services.Abstractions;

public interface ICustomAuthenticationService
{
    JwtSecurityToken GenerateToken(ClientEntity clientEntity);
}
