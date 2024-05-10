using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Services.Impl;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly JwtOptions _jwtOptions;
    private readonly BookShopDbContext _dbContext;

    public CustomAuthenticationService(JwtOptions jwtOptions, BookShopDbContext dbContext)
    {
        _jwtOptions = jwtOptions;
        _dbContext = dbContext;
    }

    public string GenerateToken(ClientEntity clientEntity)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, clientEntity.Email),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<ClientEntity> AuthenticateAsync(string email, string password)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Email == email);

        if (client == null || !VerifyPasswordHash(password, client.Password))
        {
            return null;
        }

        return client;
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return string.Equals(hashedPassword, storedHash);
        }
    }
}
