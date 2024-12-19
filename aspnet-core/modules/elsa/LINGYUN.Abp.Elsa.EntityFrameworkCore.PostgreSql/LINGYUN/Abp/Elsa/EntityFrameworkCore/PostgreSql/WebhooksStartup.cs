using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql;

[Feature("Webhooks:EntityFrameworkCore:PostgreSql")]
public class WebhooksStartup : WebhooksStartupBase
{
    protected override string ProviderName => "PostgreSql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory_Webhooks"));
    }
}
