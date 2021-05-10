using Microsoft.Extensions.DependencyInjection;
using NRules.Fluent;
using NRules.RuleModel;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules.NRules
{
    [DependsOn(
        typeof(AbpRulesModule))]
    public class AbpNRulesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IRuleRepository, RuleRepository>();

            Configure<AbpRulesOptions>(options =>
            {
                options.Contributors.Add<NRulesContributor>();
            });
        }
    }
}
