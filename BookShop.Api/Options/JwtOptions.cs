namespace BookShop.Api.Options;

public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
