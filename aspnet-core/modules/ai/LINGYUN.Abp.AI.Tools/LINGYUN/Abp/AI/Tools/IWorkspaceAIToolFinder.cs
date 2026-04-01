using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IWorkspaceAIToolFinder
{
    IDisposable DisableAITool();

    Task<AITool[]?> GetToolsAsync(WorkspaceDefinition workspace);

    bool IsAIToolEnabled();
}
