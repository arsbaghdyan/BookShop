using BookShop.Data.Entities;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Abstractions;

public interface IClientService
{
    Task RegisterAsync(ClientRegisterVm clientEntity);
    Task UpdateAsync(ClientUpdateVm clientEntity);
    Task RemoveAsync(long clientId);
    Task<ClientGetVm> GetByIdAsync(long clientId);
}
