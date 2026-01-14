using System.Collections.Generic;

namespace LINGYUN.Abp.AI.Workspaces;
public interface IWorkspaceDefinitionContext
{
    WorkspaceDefinition? GetOrNull(string name);

    IReadOnlyList<WorkspaceDefinition> GetAll();

    void Add(params WorkspaceDefinition[] definitions);
}
