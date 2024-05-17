using BookShop.Common.ClientService.Abstractions;

namespace BookShop.Api.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IClientContextReader _clientContextReader;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IClientContextReader clientContextReader)
    {
        _logger = logger;
        _clientContextReader = clientContextReader;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var clientId = _clientContextReader.GetClientContextId();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/html;charset=utf-8";
            await context.Response.WriteAsync($"Error: {ex.Message}");
            _logger.LogError(ex, $"Error: {ex.Message} : ClientId({clientId})");
        }
    }
}