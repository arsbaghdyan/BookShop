using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface IClientService
{
    Task RegisterAsync(ClientEntity clientEntity);
    Task UpdateAsync(ClientEntity clientEntity);
    Task RemoveAsync(long clientId);
    Task<ClientEntity> GetByIdAsync(long clientId);
}
