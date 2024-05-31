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

            HttpStatusCode statusCode = ex switch
            {
                InvalidProductCountException => HttpStatusCode.NotFound,
                NotEnoughProductException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var errorResponse = new BaseResponse<object>
            {
                ErrorModels = new List<ErrorModel> { new ErrorModel { ErrorMessage = ex.Message } }
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var responseJson = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(responseJson);
            _logger.LogError(ex, $"Error: {ex.Message} : ClientId({clientId})");
        }
    }
}