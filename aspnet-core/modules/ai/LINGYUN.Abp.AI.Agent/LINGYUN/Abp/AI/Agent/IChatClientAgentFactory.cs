using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Agent;
public interface IChatClientAgentFactory
{
    [NotNull]
    Task<WorkspaceAIAgent> CreateAsync<TWorkspace>();

    [NotNull]
    Task<WorkspaceAIAgent> CreateAsync(string workspace);
}
