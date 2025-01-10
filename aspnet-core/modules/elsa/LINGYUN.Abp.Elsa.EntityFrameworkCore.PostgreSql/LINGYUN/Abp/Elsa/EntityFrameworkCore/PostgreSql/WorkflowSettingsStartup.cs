using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql;

[Feature("WorkflowSettings:EntityFrameworkCore:PostgreSql")]
public class WorkflowSettingsStartup : WorkflowSettingsStartupBase
{
    protected override string ProviderName => "PostgreSql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory_WorkflowSettings"));
    }
}
