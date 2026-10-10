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

namespace LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer;

[DependsOn(
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    // Quartz SqlServer���ݿ��ʼ��ģ��
    typeof(AbpQuartzSqlServerInstallerModule),
    // Elsa������ģ�� SqlServer����
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
