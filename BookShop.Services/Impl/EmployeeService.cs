using BookShop.Services.Abstractions;
using BookShop.Services.Models.EmployeeModels;

namespace BookShop.Services.Impl;

internal class EmployeeService : IEmployeeService
{
    public Task<EmployeeModel> EmployeeLoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
}
