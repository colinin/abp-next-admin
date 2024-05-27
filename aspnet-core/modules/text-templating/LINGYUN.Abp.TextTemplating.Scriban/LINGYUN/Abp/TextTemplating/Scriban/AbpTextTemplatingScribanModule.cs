using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating.Scriban;

[DependsOn(typeof(AbpTextTemplatingCoreModule))]
public class AbpTextTemplatingScribanModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpTextTemplatingOptions>(options =>
        {
            options.DefaultRenderingEngine = ScribanTemplateRenderingEngine.EngineName;
            options.RenderingEngines[ScribanTemplateRenderingEngine.EngineName] = typeof(ScribanTemplateRenderingEngine);
        });
    }
}
