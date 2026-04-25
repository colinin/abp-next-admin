using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IChatClientProvider
{
    string Name { get; }

    ChatModel[] GetModels();

    Task<IChatClient> CreateAsync(WorkspaceDefinition workspace);
}
