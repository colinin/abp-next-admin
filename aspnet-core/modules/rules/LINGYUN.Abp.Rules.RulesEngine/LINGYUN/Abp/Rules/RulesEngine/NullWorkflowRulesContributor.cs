using RulesEngine.Models;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class NullWorkflowRulesContributor : IWorkflowRulesContributor, ISingletonDependency
    {
        public void Initialize()
        {

        }

        public Task<WorkflowRules[]> LoadAsync<T>(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new WorkflowRules[0]);
        }

        public void Shutdown()
        {

        }
    }
}
