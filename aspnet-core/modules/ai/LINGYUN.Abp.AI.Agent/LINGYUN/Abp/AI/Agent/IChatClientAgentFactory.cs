using JetBrains.Annotations;
using Microsoft.Agents.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Agent;
public interface IChatClientAgentFactory
{
    [NotNull]
    Task<ChatClientAgent> CreateAsync<TWorkspace>();

    [NotNull]
    Task<ChatClientAgent> CreateAsync(string workspace);
}
