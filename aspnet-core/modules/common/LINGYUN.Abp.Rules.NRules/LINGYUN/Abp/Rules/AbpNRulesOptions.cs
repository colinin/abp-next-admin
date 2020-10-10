using Volo.Abp.Collections;
using NRule = NRules.Fluent.Dsl.Rule;

namespace LINGYUN.Abp.Rules
{
    public class AbpNRulesOptions
    {
        public ITypeList<NRule> Rules { get; }
        public AbpNRulesOptions()
        {
            Rules = new TypeList<NRule>();
        }
    }
}
