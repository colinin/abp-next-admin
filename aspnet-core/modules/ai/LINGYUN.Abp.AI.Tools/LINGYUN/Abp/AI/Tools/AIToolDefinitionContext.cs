using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LINGYUN.Abp.AI.Tools;
public class AIToolDefinitionContext : IAIToolDefinitionContext
{
    protected Dictionary<string, AIToolDefinition> Tools { get; }

    public AIToolDefinitionContext(Dictionary<string, AIToolDefinition> tools)
    {
        Tools = tools;
    }

    public virtual AIToolDefinition? GetOrNull(string name)
    {
        return Tools.GetOrDefault(name);
    }

    public virtual IReadOnlyList<AIToolDefinition> GetAll()
    {
        return Tools.Values.ToImmutableList();
    }

    public virtual void Add(params AIToolDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Tools[definition.Name] = definition;
        }
    }
}
