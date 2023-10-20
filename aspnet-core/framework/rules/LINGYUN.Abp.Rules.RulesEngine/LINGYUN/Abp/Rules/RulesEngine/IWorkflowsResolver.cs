using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowsResolver
    {
        void Initialize(RulesInitializationContext context);

        [NotNull]
        Task<WorkflowsResolveResult> ResolveWorkflowsAsync(Type type);

        void Shutdown();
    }
}
