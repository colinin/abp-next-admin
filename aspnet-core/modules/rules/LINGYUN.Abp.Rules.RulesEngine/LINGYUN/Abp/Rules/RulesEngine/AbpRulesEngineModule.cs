using LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    [DependsOn(
        typeof(AbpRulesModule),
        typeof(AbpJsonModule))]
    public class AbpRulesEngineModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();

            Configure<AbpRulesOptions>(options =>
            {
                options.Contributors.Add<RulesEngineContributor>();
            });

            Configure<AbpRulesEngineOptions>(options =>
            {
                // 加入防止空引用
                options.Contributors.Add<NullWorkflowRulesContributor>();
                options.Contributors.Add<PhysicalFileWorkflowRulesContributor>();
            });
        }
    }
}
