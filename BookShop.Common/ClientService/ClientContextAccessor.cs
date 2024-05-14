namespace BookShop.Common.ClientService;

public class ClientContextAccessor
{
    private readonly ClientContext _clientContext;

    public ClientContextAccessor(ClientContext clientContext)
    {
        _clientContext = clientContext;
    }

    public void SetClientContextId(long Id)
    {
        _clientContext.Id = Id;
    }
}
