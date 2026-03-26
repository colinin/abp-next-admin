using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Workspaces;
public interface IWorkspaceDefinitionManager
{
    [NotNull]
    Task<WorkspaceDefinition> GetAsync([NotNull] string name);

    [ItemNotNull]
    Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync();

    [CanBeNull]
    Task<WorkspaceDefinition?> GetOrNullAsync([NotNull] string name);
}
