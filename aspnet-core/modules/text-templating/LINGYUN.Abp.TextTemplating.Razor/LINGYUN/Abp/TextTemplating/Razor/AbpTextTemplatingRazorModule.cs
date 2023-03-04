using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Razor;
using VoloAbpTextTemplatingRazorModule = Volo.Abp.TextTemplating.Razor.AbpTextTemplatingRazorModule;

namespace LINGYUN.Abp.TextTemplating.Razor;

[DependsOn(
    typeof(AbpTextTemplatingDomainModule),
    typeof(VoloAbpTextTemplatingRazorModule))]
public class AbpTextTemplatingRazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
           ServiceDescriptor.Transient<RazorTemplateRenderingEngine, RazorTextTemplateRenderingEngine>());

        Configure<AbpTextTemplatingOptions>(options =>
        {
            if (options.DefaultRenderingEngine.IsNullOrWhiteSpace())
            {
                options.DefaultRenderingEngine = RazorTemplateRenderingEngine.EngineName;
            }
            options.RenderingEngines[RazorTemplateRenderingEngine.EngineName] = typeof(RazorTextTemplateRenderingEngine);
        });
    }
}
