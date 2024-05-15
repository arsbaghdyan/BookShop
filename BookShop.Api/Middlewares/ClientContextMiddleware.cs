using BookShop.Common.ClientService.Impl;
using BookShop.Common.Consts;
using System.IdentityModel.Tokens.Jwt;

namespace BookShop.Api.Middlewares;

public class ClientContextMiddleware : IMiddleware
{
    private readonly ClientContextWriter _clientContextAccessor;

    public ClientContextMiddleware(ClientContextWriter clientContextAccessor)
    {
        _clientContextAccessor = clientContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isAutetnicated = context.User.Identity?.IsAuthenticated;

        if (isAutetnicated == true)
        {
            var tokenHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(tokenHeader))
            {
                throw new Exception("Token is missing");
            }

            var token = tokenHeader.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);

            var clientIdClaim = securityToken.Claims.FirstOrDefault(c => c.Type == BookShopClaims.Id);

            if (clientIdClaim == null)
            {
                throw new Exception("ClientId is missing");
            }

            if (!long.TryParse(clientIdClaim.Value, out long clientId))
            {
                throw new Exception("Unknown ClientId");
            }

            _clientContextAccessor.SetClientContextId(clientId);
        }

        await next(context);
    }
}
