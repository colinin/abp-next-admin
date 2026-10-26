using Elsa.Agents.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore;
using Elsa.Extensions;
using Elsa.Persistence.EFCore;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Labels;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Secrets.Persistence.EFCore;
using Elsa.UserTasks.Persistence.EFCore;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.MySql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.PostgreSql;
using LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        ConfigureElsaMigrations(context.Services);
    }

    private static void ConfigureElsaMigrations(IServiceCollection services)
    {
        AddElsaMigration<ManagementElsaDbContext>(services);
        AddElsaMigration<RuntimeElsaDbContext>(services);
        AddElsaMigration<AlterationsElsaDbContext>(services);
        AddElsaMigration<LabelsElsaDbContext>(services);
        AddElsaMigration<AgentsDbContext>(services);
        AddElsaMigration<AIDbContext>(services);
        AddElsaMigration<SecretsElsaDbContext>(services);
        AddElsaMigration<UserTasksElsaDbContext>(services);
    }

    private static void AddElsaMigration<TDbContext>(IServiceCollection services)
        where TDbContext : DbContext
    {
        services.Configure<MigrationOptions>(options =>
        {
            options.RunMigrations[typeof(TDbContext)] = true;
        });
        services.AddStartupTask<RunMigrationsStartupTask<TDbContext>>();
    }
}
