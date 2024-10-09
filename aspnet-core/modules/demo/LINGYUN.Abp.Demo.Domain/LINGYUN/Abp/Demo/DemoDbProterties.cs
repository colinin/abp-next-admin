namespace LINGYUN.Abp.Demo;
public static class DemoDbProterties
{
    public static string DbTablePrefix { get; set; } = "Demo_";

    public static string? DbSchema { get; set; } = null;


    public const string ConnectionStringName = "Demo";
}
