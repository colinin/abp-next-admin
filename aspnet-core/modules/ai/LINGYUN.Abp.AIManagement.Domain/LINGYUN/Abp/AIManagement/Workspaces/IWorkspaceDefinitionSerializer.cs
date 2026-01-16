using LINGYUN.Abp.AI.Workspaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public interface IWorkspaceDefinitionSerializer
{
    Task<Workspace[]> SerializeAsync(IEnumerable<WorkspaceDefinition> definitions);

    Task<Workspace> SerializeAsync(WorkspaceDefinition definition);
}
