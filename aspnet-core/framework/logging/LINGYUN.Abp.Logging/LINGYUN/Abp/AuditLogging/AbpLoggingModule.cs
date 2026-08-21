using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Logging;

[DependsOn(typeof(AbpSpecificationsModule))]
public class AbpLoggingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpLoggingEnricherPropertyNames>(configuration.GetSection("Logging"));
    }
}
