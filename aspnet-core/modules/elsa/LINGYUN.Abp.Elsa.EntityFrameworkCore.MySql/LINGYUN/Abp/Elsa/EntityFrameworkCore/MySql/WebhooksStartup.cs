using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[Feature("Webhooks:EntityFrameworkCore:MySql")]
public class WebhooksStartup : WebhooksStartupBase
{
    protected override string ProviderName => "MySql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            x => x.MigrationsHistoryTable("__EFMigrationsHistory_Webhooks"));
    }
}
