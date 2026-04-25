using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LINGYUN.Abp.AI.Workspaces;
public class WorkspaceDefinitionContext : IWorkspaceDefinitionContext
{
    protected Dictionary<string, WorkspaceDefinition> Workspaces { get; }

    public WorkspaceDefinitionContext(Dictionary<string, WorkspaceDefinition> workspaces)
    {
        Workspaces = workspaces;
    }

    public virtual WorkspaceDefinition? GetOrNull(string name)
    {
        return Workspaces.GetOrDefault(name);
    }

    public virtual IReadOnlyList<WorkspaceDefinition> GetAll()
    {
        return Workspaces.Values.ToImmutableList();
    }

    public virtual void Add(params WorkspaceDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Workspaces[definition.Name] = definition;
        }
    }
}
