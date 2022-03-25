namespace LINGYUN.Abp.WebhooksManagement;

public static class WebhooksManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "WebhooksManagement_";

    public static string DbSchema { get; set; } = null;


    public const string ConnectionStringName = "WebhooksManagement";
}
