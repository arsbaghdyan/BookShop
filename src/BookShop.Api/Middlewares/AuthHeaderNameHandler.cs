namespace BookShop.Api.Middlewares;

public class AuthHeaderNameHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headerValue = context.Request.Headers["JwtAuth"].ToString();
        context.Request.Headers["Authorization"] = headerValue;
        await next(context);
    }
}