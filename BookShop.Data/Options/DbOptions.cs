namespace BookShop.Data.Options;

public class DbOptions
{
    public const string Section = nameof(DbOptions);
    public string? ConnectionString { get; set; }
}
