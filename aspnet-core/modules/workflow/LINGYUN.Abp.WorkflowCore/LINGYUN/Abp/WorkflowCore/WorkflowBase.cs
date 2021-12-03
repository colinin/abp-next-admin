using WorkflowCore.Interface;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WorkflowCore
{
    public abstract class WorkflowBase : IWorkflow<WorkflowParamDictionary>, ISingletonDependency
    {
        public abstract string Id { get; }

        public abstract int Version { get; }

        public abstract void Build(IWorkflowBuilder<WorkflowParamDictionary> builder);
    }
}
