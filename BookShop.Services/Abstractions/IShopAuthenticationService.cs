using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface IShopAuthenticationService
{
    string GenerateAdminToken(EmployeeModel employee);
    string GenerateClientToken(ClientModel client);
}
