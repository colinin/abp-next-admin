using Elsa.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer;

[Feature("WorkflowSettings:EntityFrameworkCore:SqlServer")]
public class WorkflowSettingsStartup : WorkflowSettingsStartupBase
{
    protected override string ProviderName => "SqlServer";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseSqlServer(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory_WorkflowSettings"));
    }
}
