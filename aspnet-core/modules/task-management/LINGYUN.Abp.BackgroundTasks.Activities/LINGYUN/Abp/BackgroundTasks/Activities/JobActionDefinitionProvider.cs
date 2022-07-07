using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public abstract class JobActionDefinitionProvider : IJobActionDefinitionProvider, ITransientDependency
{
    public abstract void Define(IJobActionDefinitionContext context);
}
