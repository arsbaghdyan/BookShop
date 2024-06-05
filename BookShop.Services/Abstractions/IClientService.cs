using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface IClientService
{
    Task<ClientModel?> GetClientAsync();
    Task<ClientModel?> GetByEmailAndPasswordAsync(string email, string password);
    Task<ClientModel?> RegisterAsync(ClientRegisterModel clientEntity);
    Task<ClientModel?> UpdateAsync(ClientUpdateModel clientEntity);
    Task RemoveAsync();
}
