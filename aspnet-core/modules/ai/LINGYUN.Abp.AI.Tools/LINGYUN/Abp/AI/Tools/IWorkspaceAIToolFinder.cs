using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IWorkspaceAIToolFinder
{
    Task<AITool[]?> GetToolsAsync(WorkspaceDefinition workspace);
}
