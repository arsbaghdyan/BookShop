﻿using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.ClientModels;

public class ClientUpdateModel : IIdentifiable
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}