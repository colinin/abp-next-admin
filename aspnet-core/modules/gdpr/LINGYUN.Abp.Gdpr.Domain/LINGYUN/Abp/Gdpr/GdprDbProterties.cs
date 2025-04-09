namespace LINGYUN.Abp.Gdpr;
public static class GdprDbProterties
{
    public static string DbTablePrefix { get; set; } = "AbpGdpr";

    public static string? DbSchema { get; set; } = null;


    public const string ConnectionStringName = "AbpGdpr";
}
