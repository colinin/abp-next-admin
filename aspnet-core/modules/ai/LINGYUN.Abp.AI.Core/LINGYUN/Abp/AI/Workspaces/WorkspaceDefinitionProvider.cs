using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Workspaces;

public abstract class WorkspaceDefinitionProvider : IWorkspaceDefinitionProvider, ITransientDependency
{
    public abstract void Define(IWorkspaceDefinitionContext context);
}