using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Workspaces;
public class NullDynamicWorkspaceDefinitionStore : IDynamicWorkspaceDefinitionStore, ISingletonDependency
{
    private readonly static Task<WorkspaceDefinition?> CachedNullableWorkspaceResult = Task.FromResult((WorkspaceDefinition?)null);
    private readonly static Task<WorkspaceDefinition> CachedWorkspaceResult = Task.FromResult((WorkspaceDefinition)null!);

    private readonly static Task<IReadOnlyList<WorkspaceDefinition>> CachedWorkspacesResult = Task.FromResult(
        (IReadOnlyList<WorkspaceDefinition>)Array.Empty<WorkspaceDefinition>().ToImmutableList());

    public Task<WorkspaceDefinition> GetAsync(string name)
    {
        return CachedWorkspaceResult;
    }

    public Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        return CachedWorkspacesResult;
    }

    public Task<WorkspaceDefinition?> GetOrNullAsync(string name)
    {
        return CachedNullableWorkspaceResult;
    }
}
