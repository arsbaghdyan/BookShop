using BookShop.Common.Consts;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.EmployeeModels;
using BookShop.Services.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookShop.Services.Impl;

public class ShopAuthenticationService : IShopAuthenticationService
{
    private readonly ClientJwtOptions _clientJwtOptions;
    private readonly AdminJwtOptions _adminJwtOptions;

    public ShopAuthenticationService(ClientJwtOptions jwtOptions,
        AdminJwtOptions adminJwtOptions)
    {
        _clientJwtOptions = jwtOptions;
        _adminJwtOptions = adminJwtOptions;
    }

    public string GenerateClientToken(ClientModel client)
    {
        var claims = new List<Claim>
        {
            new Claim(BookShopClaims.ClientId, client.Id.ToString()),
            new Claim(ClaimTypes.Email, client.Email)
        };

        return GenerateTokenInternal(claims, _clientJwtOptions);
    }

    public string GenerateAdminToken(EmployeeModel employee)
    {
        var claims = new List<Claim>
        {
            new Claim(BookShopClaims.EmployeeId, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.Email)
        };

        return GenerateTokenInternal(claims, _adminJwtOptions);
    }

    private string GenerateTokenInternal(List<Claim> claims, JwtOptions jwtOptions)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.ExpirationInSeconds),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
