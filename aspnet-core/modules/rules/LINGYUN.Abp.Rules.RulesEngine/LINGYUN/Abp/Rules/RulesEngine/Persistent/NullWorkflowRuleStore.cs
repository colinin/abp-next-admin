using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine.Persistent
{
    [Dependency(TryRegister = true)]
    public class NullWorkflowRuleStore : IWorkflowRuleStore, ISingletonDependency
    {
        public Task<IEnumerable<WorkflowRules>> GetRulesAsync(Type inputType, CancellationToken cancellationToken = default)
        {
            IEnumerable<WorkflowRules> rules = new WorkflowRules[0];
            return Task.FromResult(rules);
        }
    }
}
