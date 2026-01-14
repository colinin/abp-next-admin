using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Workspaces;
public interface IWorkspaceDefinitionManager
{
    [ItemNotNull]
    Task<WorkspaceDefinition> GetAsync([NotNull] string name);

    [ItemNotNull]
    Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync();

    [ItemCanBeNull]
    Task<WorkspaceDefinition?> GetOrNullAsync([NotNull] string name);
}
