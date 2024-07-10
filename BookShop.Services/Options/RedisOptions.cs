namespace BookShop.Services.Options;

public class RedisOptions
{
    public const string SectionName = nameof(RedisOptions);
    public string Configuration { get; set; }
    public string Password { get; set; }
}
