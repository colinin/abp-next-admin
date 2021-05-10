using Volo.Abp.Collections;

namespace LINGYUN.Abp.Rules
{
    public class AbpRulesOptions
    {
        public ITypeList<IRuleContributor> Contributors { get; }

        public AbpRulesOptions()
        {
            Contributors = new TypeList<IRuleContributor>();
        }
    }
}
