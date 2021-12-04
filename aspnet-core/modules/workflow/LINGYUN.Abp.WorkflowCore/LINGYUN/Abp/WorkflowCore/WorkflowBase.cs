using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    public abstract class WorkflowBase : IWorkflow, ISingletonDependency
    {
        public abstract string Id { get; }

        public abstract int Version { get; }

        public abstract void Build(IWorkflowBuilder<object> builder);
    }
}
