namespace BookShop.Data.Options;

public class DbOptions
{
    public const string SectionName = nameof(DbOptions);
    public string? ConnectionString { get; set; }
}
