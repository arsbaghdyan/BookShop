using BookShop.Services.Abstractions;

namespace BookShop.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICustomAuthenticationService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token != null)
            {
                var clientEmail = authService.GetClientEmailFromToken(token);
                context.Items["ClientEmail"] = clientEmail;
            }
            await _next(context);
        }
    }
}
