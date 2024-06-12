using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.EmployeeModels;

namespace BookShop.Services.Abstractions;

public interface IShopAuthenticationService
{
    string GenerateAdminToken(EmployeeModel employee);
    string GenerateClientToken(ClientModel client);
}
