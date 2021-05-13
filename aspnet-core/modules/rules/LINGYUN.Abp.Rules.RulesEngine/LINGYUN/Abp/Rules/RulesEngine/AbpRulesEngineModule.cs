using LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical;
using LINGYUN.Abp.Rules.RulesEngine.Persistent;
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

            Configure<AbpRulesEngineResolveOptions>(options =>
            {
                options.WorkflowRulesResolvers.Add(new PersistentWorkflowRulesResolveContributor());
                options.WorkflowRulesResolvers.Add(new PhysicalFileWorkflowRulesResolveContributor());
            });
        }
    }
}
