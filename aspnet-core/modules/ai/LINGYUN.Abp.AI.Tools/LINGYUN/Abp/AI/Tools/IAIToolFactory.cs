using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolFactory
{
    Task<AITool[]> CreateTool(AIToolDefinition definition);

    Task<AITool[]> CreateAllTools();
}
