﻿namespace BookShop.Services.Models.PageModels;

public class ProductPageModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; } = "name";
    public string OrderDirection { get; set; } = "asc";
}