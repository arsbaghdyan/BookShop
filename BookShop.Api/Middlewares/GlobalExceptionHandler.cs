using BookShop.Api.ExceptionHandler;
using BookShop.Api.Models.ErrorModels;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Services.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace BookShop.Api.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IClientContextReader _clientContextReader;
    private readonly IHostEnvironment _hostEnvironment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
        IClientContextReader clientContextReader,
        IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _clientContextReader = clientContextReader;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Something went wrong.";

        if (_hostEnvironment.IsDevelopment())
        {
            message += $" {ex.Message}";
        }

        switch (ex)
        {
            case InvalidProductCountException:
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                break;
            case NotEnoughProductException:
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                break;

            default:
                break;
        }

        var errorResponse = new BaseResponse<object>
        {
            Data = null,
            Success = false,
            ErrorModels = new List<ErrorModel> { new ErrorModel { ErrorMessage = message } }
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var responseJson = JsonConvert.SerializeObject(errorResponse);

        await context.Response.WriteAsync(responseJson);
        _logger.LogError(ex, $"Error: {ex.Message} : ClientId({clientId})");
    }
}