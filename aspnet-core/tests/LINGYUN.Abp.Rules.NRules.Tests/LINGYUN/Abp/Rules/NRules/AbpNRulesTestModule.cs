using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules.NRules
{
    [DependsOn(
        typeof(AbpNRulesModule))]
    public class AbpNRulesTestModule : AbpModule
    {
    }
}
