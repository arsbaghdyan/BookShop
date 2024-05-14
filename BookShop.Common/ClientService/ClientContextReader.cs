namespace BookShop.Common.ClientService;

public class ClientContextReader
{
    private readonly ClientContext _clientContext;

    public ClientContextReader(ClientContext clientContext)
    {
        _clientContext = clientContext;
    }

    public long GetClientContextId()
    {
        return _clientContext.Id;
    }
}
