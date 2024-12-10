using LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql;

[DependsOn(
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpElsaEntityFrameworkCorePostgreSqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCorePostgreSqlModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            options.UseNpgsql();
        });
    }
}
