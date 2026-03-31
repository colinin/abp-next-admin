using JetBrains.Annotations;
using Microsoft.Agents.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Agent;
public interface IAgentFactory
{
    [NotNull]
    Task<AIAgent> CreateAsync<TWorkspace>();

    [NotNull]
    Task<AIAgent> CreateAsync(string workspace);
}
