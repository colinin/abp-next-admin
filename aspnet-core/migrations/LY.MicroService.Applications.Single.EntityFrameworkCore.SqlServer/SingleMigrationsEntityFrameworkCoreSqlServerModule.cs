using DotNetCore.CAP;
using LINGYUN.Abp.Quartz.SqlServerInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.SqlServer;

[DependsOn(
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    // Quartz SqlServer数据库初始化模块
    typeof(AbpQuartzSqlServerInstallerModule),
    // Elsa工作流模块 SqlServer集成
    //typeof(AbpElsaEntityFrameworkCoreSqlServerModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCoreSqlServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("SqlServer".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
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
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("SqlServer".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer(sqlServer =>
                {
                    sqlServer.MigrationsAssembly(GetType().Assembly);
                });
            });

            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd;
                }
            });
        }
    }
}
