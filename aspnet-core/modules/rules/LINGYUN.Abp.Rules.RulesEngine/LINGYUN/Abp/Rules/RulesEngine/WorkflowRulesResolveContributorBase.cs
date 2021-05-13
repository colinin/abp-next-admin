using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public abstract class WorkflowRulesResolveContributorBase : IWorkflowRulesResolveContributor
    {
        public abstract string Name { get; }

        public virtual void Initialize(RulesInitializationContext context)
        {
        }
        public abstract Task ResolveAsync(IWorkflowRulesResolveContext context);

        public virtual void Shutdown()
        {
        }
    }
}
