using Microsoft.Extensions.AI;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolProvider
{
    string Name { get; }

    AITool CreateTool(AIToolDefinition definition);
}
