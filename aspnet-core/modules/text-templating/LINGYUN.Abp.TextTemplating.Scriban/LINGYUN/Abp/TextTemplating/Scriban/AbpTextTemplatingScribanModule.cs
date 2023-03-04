using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Scriban;
using VoloAbpTextTemplatingScribanModule = Volo.Abp.TextTemplating.Scriban.AbpTextTemplatingScribanModule;

namespace LINGYUN.Abp.TextTemplating.Scriban;

[DependsOn(
    typeof(AbpTextTemplatingDomainModule),
    typeof(VoloAbpTextTemplatingScribanModule))]
public class AbpTextTemplatingScribanModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<ScribanTemplateRenderingEngine, ScribanTextTemplateRenderingEngine>());

        Configure<AbpTextTemplatingOptions>(options =>
        {
            options.DefaultRenderingEngine = ScribanTemplateRenderingEngine.EngineName;
            options.RenderingEngines[ScribanTemplateRenderingEngine.EngineName] = typeof(ScribanTextTemplateRenderingEngine);
        });
    }
}
