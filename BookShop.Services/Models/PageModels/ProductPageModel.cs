using System.ComponentModel.DataAnnotations;

namespace BookShop.Services.Models.PageModels;

public class ProductPageModel
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    [Range(1, 30)]
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; }
    public bool IsOrderAsc { get; set; }
}
