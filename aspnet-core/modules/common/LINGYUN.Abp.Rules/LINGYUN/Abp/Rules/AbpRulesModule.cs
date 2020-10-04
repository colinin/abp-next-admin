using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules
{
    [DependsOn(
        typeof(AbpDddDomainModule))]
    public class AbpRulesModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(EntityChangedRulesInterceptorRegistrar.RegisterIfNeeded);
        }
    }
}
