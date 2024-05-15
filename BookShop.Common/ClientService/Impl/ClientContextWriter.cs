using BookShop.Common.ClientService.Abstractions;
namespace BookShop.Common.ClientService.Impl;

public class ClientContextWriter : IClientContextWriter
{
    private readonly ClientContext _clientContext;

    public ClientContextWriter(ClientContext clientContext)
    {
        _clientContext = clientContext;
    }

    public void SetClientContextId(long Id)
    {
        _clientContext.Id = Id;
    }
}
