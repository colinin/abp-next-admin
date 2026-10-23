using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.PostgreSql;
using LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.WorkflowService;

[DependsOn(
    typeof(AbpElsaNextApprovalFlowEntityFrameworkCoreModule),
    typeof(AbpElsaNextUserTasksEntityFrameworkCorePostgreSqlModule),
    typeof(AbpElsaNextEntityFrameworkCorePostgreSqlModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpDataDbMigratorModule))]
public class WorkflowServiceMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WorkflowServiceMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
    }
}
