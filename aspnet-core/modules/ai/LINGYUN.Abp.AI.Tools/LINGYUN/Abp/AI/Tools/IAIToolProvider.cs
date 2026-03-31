using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolProvider
{
    string Name { get; }

    AIToolPropertyDescriptor[] GetPropertites();

    Task<AITool[]> CreateToolsAsync(AIToolDefinition definition);
}
