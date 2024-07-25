namespace BookShop.Services.Options;

public abstract class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInSeconds { get; set; }
}
