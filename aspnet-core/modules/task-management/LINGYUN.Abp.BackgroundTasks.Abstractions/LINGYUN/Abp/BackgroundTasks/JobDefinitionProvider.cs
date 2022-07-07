using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

public abstract class JobDefinitionProvider : IJobDefinitionProvider, ITransientDependency
{
    public abstract void Define(IJobDefinitionContext context);
}
