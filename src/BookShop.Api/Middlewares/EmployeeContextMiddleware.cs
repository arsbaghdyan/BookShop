using BookShop.Api.Constants;
using BookShop.Common.Consts;
using BookShop.Common.EmployeeService.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace BookShop.Api.Middlewares;

public class EmployeeContextMiddleware : IMiddleware
{
    private readonly IEmployeeContextWriter _employeeContextWriter;

    public EmployeeContextMiddleware(IEmployeeContextWriter employeeContextWriter)
    {
        _employeeContextWriter = employeeContextWriter;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isAuthenticated = context.User.Identity?.IsAuthenticated;

        if (isAuthenticated == true)
        {
            var authenticateResultFeature = context.Features.Get<IAuthenticateResultFeature>();

            if (authenticateResultFeature?.AuthenticateResult?.Ticket?.AuthenticationScheme !=
                AuthSchemas.AdminFlow)
            {
                await next(context);
                return;
            }

            var tokenHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(tokenHeader))
            {
                throw new Exception("Token is missing");
            }
            var token = tokenHeader.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);

            var employeeIdClaim = securityToken.Claims.FirstOrDefault(c => c.Type == BookShopClaims.EmployeeId);

            if (employeeIdClaim == null)
            {
                throw new Exception("EmployeeId is missing");
            }

            if (!long.TryParse(employeeIdClaim.Value, out long employeeId))
            {
                throw new Exception("Unknown employeeId");
            }

            _employeeContextWriter.SetEmployeeContextId(employeeId);
        }

        await next(context);
    }
}
