using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.UserTasks.Persistence.EFCore.Extensions;
using Elsa.UserTasks.Persistence.EFCore.Features;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore;

[DependsOn(typeof(AbpElsaNextUserTasksModule))]
public class AbpElsaNextUserTasksEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("UserTasks:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreUserTasksPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseUserTasks(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }
}
