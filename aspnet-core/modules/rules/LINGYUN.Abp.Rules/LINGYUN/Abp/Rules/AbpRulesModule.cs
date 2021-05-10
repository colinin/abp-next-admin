using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Rules
{
    [DependsOn(
        typeof(AbpMultiTenancyModule))]
    public class AbpRulesModule : AbpModule
    {
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<RuleProvider>()
                .Initialize();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            context.ServiceProvider
                .GetRequiredService<RuleProvider>()
                .Shutdown();
        }
    }
}
