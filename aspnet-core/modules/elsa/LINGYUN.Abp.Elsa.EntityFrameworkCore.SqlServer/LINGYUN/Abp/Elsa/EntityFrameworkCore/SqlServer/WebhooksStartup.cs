using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer;

[Feature("Webhooks:EntityFrameworkCore:SqlServer")]
public class WebhooksStartup : WebhooksStartupBase
{
    protected override string ProviderName => "SqlServer";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseSqlServer(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory_Webhooks"));
    }
}
