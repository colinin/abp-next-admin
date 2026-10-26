using Elsa.Extensions;
using Elsa.Persistence.EFCore;
using Elsa.Scheduling.Quartz.EFCore.MySql;
using Elsa.Scheduling.Quartz.EFCore.PostgreSql;
using Elsa.Scheduling.Quartz.EFCore.SqlServer;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.MySql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.PostgreSql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AllInOne.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SingleMigrationsEntityFrameworkCorePostgreSqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreSqlServerModule),
    typeof(SingleMigrationsEntityFrameworkCoreMySqlModule)
    )]
public partial class SingleDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureTiming(configuration);
        ConfigureElsaQuartzMigrations(context, configuration);
    }

    private static void ConfigureElsaQuartzMigrations(
        ServiceConfigurationContext context,
        IConfiguration configuration)
    {
        var provider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        var connectionString = configuration.GetConnectionString("Default");
        if (connectionString.IsNullOrWhiteSpace())
        {
            return;
        }

        switch (provider?.ToLowerInvariant())
        {
            case "mysql":
                context.Services.AddDbContextFactory<MySqlQuartzDbContext>(
                    options => options.UseMySql(
                        connectionString,
                        new MySqlServerVersion(new Version(8, 0, 0))));
                context.Services.Configure<MigrationOptions>(
                    options => options.RunMigrations[typeof(MySqlQuartzDbContext)] = true);
                context.Services.AddStartupTask<RunMigrationsStartupTask<MySqlQuartzDbContext>>();
                break;
            case "postgresql":
                context.Services.AddDbContextFactory<PostgreSqlQuartzDbContext>(
                    options => options.UseNpgsql(connectionString));
                context.Services.Configure<MigrationOptions>(
                    options => options.RunMigrations[typeof(PostgreSqlQuartzDbContext)] = true);
                context.Services.AddStartupTask<RunMigrationsStartupTask<PostgreSqlQuartzDbContext>>();
                break;
            case "sqlserver":
                context.Services.AddDbContextFactory<SqlServerQuartzDbContext>(
                    options => options.UseSqlServer(connectionString));
                context.Services.Configure<MigrationOptions>(
                    options => options.RunMigrations[typeof(SqlServerQuartzDbContext)] = true);
                context.Services.AddStartupTask<RunMigrationsStartupTask<SqlServerQuartzDbContext>>();
                break;
        }
    }
}
