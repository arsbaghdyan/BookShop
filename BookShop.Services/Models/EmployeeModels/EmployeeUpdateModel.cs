﻿namespace BookShop.Services.Models.EmployeeModels;

public class EmployeeUpdateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Position { get; set; }
    public decimal Salary { get; set; }
}
