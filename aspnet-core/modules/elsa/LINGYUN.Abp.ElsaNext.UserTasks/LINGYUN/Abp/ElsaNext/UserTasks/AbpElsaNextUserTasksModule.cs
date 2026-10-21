using Elsa.Extensions;
using Elsa.Features.Services;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.ElsaNext.UserTasks;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextUserTasksModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa");
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseUserTasks(userTasks =>
            {
                userTasks.ConfigureOptions = options =>
                {
                    options.TenantClaimType = AbpClaimTypes.TenantId;
                    options.DisplayNameClaimType = AbpClaimTypes.Name;
                    configuration.GetSection("UserTasks").Bind(options);
                };
            });
        });
    }
}
