using BookShop.Services.Models.EmployeeModels;

namespace BookShop.Services.Abstractions;

public interface IEmployeeService
{
    Task<EmployeeModel> EmployeeLoginAsync(string email, string password);

}
