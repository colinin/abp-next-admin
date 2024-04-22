using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public abstract class WorkflowsResolveContributorBase : IWorkflowsResolveContributor
    {
        public abstract string Name { get; }

        public virtual void Initialize(RulesInitializationContext context)
        {
        }
        public abstract Task ResolveAsync(IWorkflowsResolveContext context);

        public virtual void Shutdown()
        {
        }
    }
}
