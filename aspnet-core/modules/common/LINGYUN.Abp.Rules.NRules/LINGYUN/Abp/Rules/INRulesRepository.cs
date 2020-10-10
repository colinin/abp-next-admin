using NRules.RuleModel;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules
{
    public interface INRulesRepository : IRuleRepository
    {
        void Add(IRuleSet ruleSet);

        void Remove(string ruleSetName);

        void Remove(IRuleSet ruleSet);

        IRuleSet GetRuleSet(string ruleSetName);

        IRuleSet GetRuleSet(RuleGroup group);

        IEnumerable<IRuleSet> GetRuleSets(IEnumerable<RuleGroup> groups);
    }
}
