using LINGYUN.Abp.AI.Workspaces;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IChatClientProvider
{
    string Name { get; }

    Task<IWorkspaceChatClient> CreateAsync(WorkspaceDefinition workspace);
}
