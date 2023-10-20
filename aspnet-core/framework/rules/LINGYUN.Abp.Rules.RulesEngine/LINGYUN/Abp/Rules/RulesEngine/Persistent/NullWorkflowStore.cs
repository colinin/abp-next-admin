using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine.Persistent
{
    [Dependency(TryRegister = true)]
    public class NullWorkflowStore : IWorkflowStore, ISingletonDependency
    {
        public Task<Workflow> GetWorkflowAsync(string workflowName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Workflow>(null);
        }

        public Task<IEnumerable<Workflow>> GetWorkflowsAsync(Type inputType, CancellationToken cancellationToken = default)
        {
            IEnumerable<Workflow> rules = new Workflow[0];
            return Task.FromResult(rules);
        }
    }
}
