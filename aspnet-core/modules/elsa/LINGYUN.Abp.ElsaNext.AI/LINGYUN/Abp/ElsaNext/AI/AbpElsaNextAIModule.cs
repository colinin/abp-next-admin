using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.AI.Extensions;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.AI;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextAIModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:AI");
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAbpAI(ai =>
            {
                ai.ConfigureOptions = options => configuration.GetSection("Host").Bind(options);
                ai.ConfigureCopilotOptions = options => configuration.GetSection("Copilot").Bind(options);
            });
        });
    }
}
