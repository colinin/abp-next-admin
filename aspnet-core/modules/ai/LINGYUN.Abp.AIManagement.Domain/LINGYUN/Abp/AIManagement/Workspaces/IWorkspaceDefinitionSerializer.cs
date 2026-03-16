using LINGYUN.Abp.AI.Workspaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public interface IWorkspaceDefinitionSerializer
{
    Task<WorkspaceDefinitionRecord[]> SerializeAsync(IEnumerable<WorkspaceDefinition> definitions);

    Task<WorkspaceDefinitionRecord> SerializeAsync(WorkspaceDefinition definition);
}
