using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[Feature("WorkflowSettings:EntityFrameworkCore:MySql")]
public class WorkflowSettingsStartup : WorkflowSettingsStartupBase
{
    protected override string ProviderName => "MySql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
#pragma warning disable CS0618
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mysql => mysql.MigrationsHistoryTable("__EFMigrationsHistory_WorkflowSettings")
                .TranslateParameterizedCollectionsToConstants());
#pragma warning restore CS0618
    }
}
