using Elsa.UserTasks.Persistence.EFCore.Features;
using Elsa.UserTasks.Persistence.EFCore.MySql.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.MySQL;

[DependsOn(typeof(AbpElsaNextUserTasksEntityFrameworkCoreModule))]
public class AbpElsaNextUserTasksEntityFrameworkCoreMySQLModule : AbpModule
{
    private const string DefaultConnectionString = "Server=127.0.0.1;Database=elsa;User Id=root;Password=123456;";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("UserTasks:EfCore:MySQL"))
        {
            PreConfigure<EFCoreUserTasksPersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:UserTasks:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }
}
