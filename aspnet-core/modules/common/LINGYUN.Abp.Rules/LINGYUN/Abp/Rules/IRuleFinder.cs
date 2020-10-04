using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules
{
    public interface IRuleFinder
    {
        Task<List<RuleGroup>> GetRuleGroupsAsync(Type entityType);
    }
}
