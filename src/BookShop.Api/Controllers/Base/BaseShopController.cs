using BookShop.Api.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers.Base;

[ApiController]
public class BaseShopController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var responseBase = new BaseResponse<object>
        {
            Success = true,
            Data = value
        };

        return base.Ok(responseBase);
    }
}
