using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Workspaces;
public interface IDynamicWorkspaceDefinitionStore
{
    Task<WorkspaceDefinition> GetAsync([NotNull] string name);

    Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync();

    Task<WorkspaceDefinition?> GetOrNullAsync([NotNull] string name);
}
