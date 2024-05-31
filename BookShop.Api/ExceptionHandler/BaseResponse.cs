using BookShop.Api.Models.ErrorModels;

namespace BookShop.Api.ExceptionHandler;

public class BaseResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public List<ErrorModel> ErrorModels { get; set; }
}
