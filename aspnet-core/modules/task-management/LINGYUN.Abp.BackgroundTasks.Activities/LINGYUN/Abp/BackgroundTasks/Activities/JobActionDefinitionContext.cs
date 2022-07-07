using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LINGYUN.Abp.BackgroundTasks.Activities;
public class JobActionDefinitionContext : IJobActionDefinitionContext
{
    protected Dictionary<string, JobActionDefinition> Actions { get; }

    public JobActionDefinitionContext(Dictionary<string, JobActionDefinition> actions)
    {
        Actions = actions;
    }

    public virtual JobActionDefinition GetOrNull(string name)
    {
        return Actions.GetOrDefault(name);
    }

    public virtual IReadOnlyList<JobActionDefinition> GetAll()
    {
        return Actions.Values.ToImmutableList();
    }

    public virtual void Add(params JobActionDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Actions[definition.Name] = definition;
        }
    }
}
