using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface IClientService
{
    Task RegisterAsync(ClientRegisterModel clientEntity);
    Task UpdateAsync(ClientUpdateModel clientEntity);
    Task RemoveAsync(long clientId);
    Task<ClientModel?> GetByIdAsync(long clientId);
    Task<ClientModel?> GetByEmailAndPasswordAsync(string email, string password);
}
