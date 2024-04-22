using LINGYUN.Abp.Rules.RulesEngine.FileProviders.Physical;
using LINGYUN.Abp.Rules.RulesEngine.Persistent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulesEngine.Interfaces;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Engine = RulesEngine.RulesEngine;

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
                options.WorkflowsResolvers.Add(new PersistentWorkflowsResolveContributor());
                options.WorkflowsResolvers.Add(new PhysicalFileWorkflowsResolveContributor());
            });

            context.Services.AddSingleton<IRulesEngine>((sp) =>
            {
                var options = sp.GetRequiredService<IOptions<AbpRulesEngineOptions>>().Value;
                return new Engine(options.Settings);
            });
        }
    }
}
