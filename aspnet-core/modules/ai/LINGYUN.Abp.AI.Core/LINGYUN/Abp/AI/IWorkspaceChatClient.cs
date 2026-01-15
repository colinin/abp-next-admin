using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;

namespace LINGYUN.Abp.AI;
public interface IWorkspaceChatClient : IChatClient
{
    WorkspaceDefinition? Workspace {  get; }
}
