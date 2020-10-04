using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules
{
    public class NullRuleFinder : IRuleFinder, ISingletonDependency
    {
        public Task<List<RuleGroup>> GetRuleGroupsAsync(Type entityType)
        {
            return Task.FromResult(new List<RuleGroup>());
        }
    }
}
