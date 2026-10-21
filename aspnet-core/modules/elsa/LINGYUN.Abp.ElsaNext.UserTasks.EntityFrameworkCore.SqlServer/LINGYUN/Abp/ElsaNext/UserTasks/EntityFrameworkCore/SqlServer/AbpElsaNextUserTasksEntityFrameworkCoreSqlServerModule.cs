using Elsa.UserTasks.Persistence.EFCore.Features;
using Elsa.UserTasks.Persistence.EFCore.SqlServer.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.SqlServer;

[DependsOn(typeof(AbpElsaNextUserTasksEntityFrameworkCoreModule))]
public class AbpElsaNextUserTasksEntityFrameworkCoreSqlServerModule : AbpModule
{
    private const string DefaultConnectionString = "Server=127.0.0.1;Database=elsa;User Id=sa;Password=123456;";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("UserTasks:EfCore:SqlServer"))
        {
            PreConfigure<EFCoreUserTasksPersistenceFeature>(efCore =>
            {
                efCore.UseSqlServer(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:UserTasks:EfCore:SqlServer:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }
}
