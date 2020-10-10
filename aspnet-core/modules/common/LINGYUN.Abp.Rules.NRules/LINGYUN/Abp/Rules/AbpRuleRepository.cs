using NRules.RuleModel;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules
{
    public class AbpRuleRepository : INRulesRepository
    {
        public void Add(IRuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRuleSet> GetRuleSets()
        {
            throw new NotImplementedException();
        }

        public void Remove(string ruleSetName)
        {
            throw new NotImplementedException();
        }

        public void Remove(IRuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        public IRuleSet GetRuleSet(string ruleSetName)
        {
            return new RuleSet(ruleSetName);
        }

        public IRuleSet GetRuleSet(RuleGroup group)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRuleSet> GetRuleSets(IEnumerable<RuleGroup> groups)
        {
            throw new NotImplementedException();
        }
    }
}
