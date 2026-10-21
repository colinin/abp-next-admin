using Elsa.UserTasks.Persistence.EFCore.Features;
using Elsa.UserTasks.Persistence.EFCore.Sqlite.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.Sqlite;

[DependsOn(typeof(AbpElsaNextUserTasksEntityFrameworkCoreModule))]
public class AbpElsaNextUserTasksEntityFrameworkCoreSqliteModule : AbpModule
{
    private const string DefaultConnectionString = "Data Source=elsa.sqlite.db;Cache=Shared;";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("UserTasks:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreUserTasksPersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:UserTasks:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }
}
