// using LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer;
using DotNetCore.CAP;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer;
using LINGYUN.Abp.Quartz.SqlServerInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.SqlServer;

[DependsOn(
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    // Quartz SqlServer数据库初始化模块
    typeof(AbpQuartzSqlServerInstallerModule),
    // Elsa工作流模块 SqlServer集成
    typeof(AbpElsaEntityFrameworkCoreSqlServerModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCoreSqlServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<CapOptions>(options =>
        {
            if (configuration.GetValue<bool>("CAP:IsEnabled"))
            {
                options.UseSqlServer(
                    sqlOptions =>
                    {
                        configuration.GetSection("CAP:SqlServer").Bind(sqlOptions);
                    });
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
    }
}
