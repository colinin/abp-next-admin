using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IChatClientProvider
{
    string Name { get; }

    Task<IChatClient> CreateAsync(WorkspaceDefinition workspace);
}
