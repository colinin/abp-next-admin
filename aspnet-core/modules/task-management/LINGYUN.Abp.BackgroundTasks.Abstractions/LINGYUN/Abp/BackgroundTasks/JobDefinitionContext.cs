using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobDefinitionContext : IJobDefinitionContext
{
    protected Dictionary<string, JobDefinition> Jobs { get; }

    public JobDefinitionContext(Dictionary<string, JobDefinition> jobs)
    {
        Jobs = jobs;
    }

    public virtual JobDefinition GetOrNull(string name)
    {
        return Jobs.GetOrDefault(name);
    }

    public virtual IReadOnlyList<JobDefinition> GetAll()
    {
        return Jobs.Values.ToImmutableList();
    }

    public virtual void Add(params JobDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Jobs[definition.Name] = definition;
        }
    }
}
