using RulesEngine.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowRulesContributor
    {
        void Initialize();

        Task<WorkflowRules[]> LoadAsync<T>(CancellationToken cancellationToken = default);

        void Shutdown();
    }
}
