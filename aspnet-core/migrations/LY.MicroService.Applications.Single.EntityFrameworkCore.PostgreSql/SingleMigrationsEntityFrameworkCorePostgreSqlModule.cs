using DotNetCore.CAP;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql;
using LINGYUN.Abp.Quartz.PostgresSqlInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql;

[DependsOn(
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpQuartzPostgresSqlInstallerModule),
    typeof(AbpElsaEntityFrameworkCorePostgreSqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCorePostgreSqlModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("PostgreSql".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var configuration = context.Services.GetConfiguration();

            PreConfigure<CapOptions>(options =>
            {
                if (configuration.GetValue<bool>("CAP:IsEnabled"))
                {
                    options.UsePostgreSql(
                        sqlOptions =>
                        {
                            configuration.GetSection("CAP:PostgreSql").Bind(sqlOptions);
                        });
                }
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("PostgreSql".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(npgsql => npgsql.MigrationsAssembly(GetType().Assembly));
            });

            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
                }
            });
        }
    }
}
