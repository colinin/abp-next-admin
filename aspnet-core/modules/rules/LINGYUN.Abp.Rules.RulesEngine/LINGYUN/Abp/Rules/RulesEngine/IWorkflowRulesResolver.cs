using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public interface IWorkflowRulesResolver
    {
        void Initialize(RulesInitializationContext context);

        [NotNull]
        Task<WorkflowRulesResolveResult> ResolveWorkflowRulesAsync(Type type);

        void Shutdown();
    }
}
