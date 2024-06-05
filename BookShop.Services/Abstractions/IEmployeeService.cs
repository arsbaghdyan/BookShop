using BookShop.Services.Models.EmployeeModels;

namespace BookShop.Services.Abstractions;

public interface IEmployeeService
{
    Task<EmployeeModel> EmployeeLoginAsync(string email, string password);
    Task<EmployeeModel> RegisterAsync(EmployeeRegisterModel employeeRegisterModel);
    Task<EmployeeModel> UpdateAsync(EmployeeUpdateModel employeeUpdateModel);
    Task RemoveAsync();
}
