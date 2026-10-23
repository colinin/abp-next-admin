using Elsa.Agents;
using Elsa.Extensions;
using Elsa.Features.Services;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Agents;

[DependsOn(
    typeof(AbpElsaNextModule))]
public class AbpElsaNextAgentsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAgents();
            elsa.UseAgentActivities();

            elsa.UseAgentsApi();
        });
    }
}
