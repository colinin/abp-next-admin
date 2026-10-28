using Elsa.UserTasks.Persistence.EFCore.Features;
using Elsa.UserTasks.Persistence.EFCore.PostgreSql.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.PostgreSql;

[DependsOn(typeof(AbpElsaNextUserTasksEntityFrameworkCoreModule))]
public class AbpElsaNextUserTasksEntityFrameworkCorePostgreSqlModule : AbpModule
{
    private const string DefaultConnectionString = "Host=127.0.0.1;Database=elsa;Username=postgres;Password=123456;";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("UserTasks:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreUserTasksPersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:UserTasks:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }
}
