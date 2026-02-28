using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[Feature("Webhooks:EntityFrameworkCore:MySql")]
public class WebhooksStartup : WebhooksStartupBase
{
    protected override string ProviderName => "MySql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
#pragma warning disable CS0618
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mysql => mysql.MigrationsHistoryTable("__EFMigrationsHistory_Webhooks")
                .TranslateParameterizedCollectionsToConstants());
#pragma warning restore CS0618
    }
}
