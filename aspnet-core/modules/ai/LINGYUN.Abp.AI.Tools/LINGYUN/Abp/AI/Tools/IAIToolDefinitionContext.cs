using System.Collections.Generic;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolDefinitionContext
{
    AIToolDefinition? GetOrNull(string name);

    IReadOnlyList<AIToolDefinition> GetAll();

    void Add(params AIToolDefinition[] definitions);
}
