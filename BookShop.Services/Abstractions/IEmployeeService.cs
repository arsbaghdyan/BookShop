using BookShop.Services.Models.EmployeeModels;

namespace BookShop.Services.Abstractions;

public interface IEmployeeService
{
    Task<EmployeeModel?> GetAdminByEmailAndPasswordAsync(string email, string password);
    Task<EmployeeModel?> GetEmployeeByEmailAndPasswordAsync(string email, string password);
    Task<EmployeeModel> RegisterAsync(EmployeeRegisterModel employeeRegisterModel);
    Task<EmployeeModel> UpdateAsync(EmployeeUpdateModel employeeUpdateModel);
    Task RemoveAsync();
}
