using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    public abstract class WorkflowBase : IWorkflow
    {
        public abstract string Id { get; }

        public abstract int Version { get; }

        public abstract void Build(IWorkflowBuilder<object> builder);
    }

    public abstract class WorkflowBase<TData> : IWorkflow<TData>
        where TData: new()
    {
        public abstract string Id { get; }

        public abstract int Version { get; }

        public abstract void Build(IWorkflowBuilder<TData> builder);
    }
}
