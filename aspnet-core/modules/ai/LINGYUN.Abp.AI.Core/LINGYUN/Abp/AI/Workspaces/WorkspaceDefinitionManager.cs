using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Workspaces;
public class WorkspaceDefinitionManager : IWorkspaceDefinitionManager, ISingletonDependency
{
    protected readonly IStaticWorkspaceDefinitionStore StaticStore;
    protected readonly IDynamicWorkspaceDefinitionStore DynamicStore;

    public WorkspaceDefinitionManager(
        IStaticWorkspaceDefinitionStore staticStore, 
        IDynamicWorkspaceDefinitionStore dynamicStore)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<WorkspaceDefinition> GetAsync(string name)
    {
        var workspace = await GetOrNullAsync(name);
        if (workspace == null)
        {
            throw new AbpException("Undefined Workspace: " + name);
        }

        return workspace;
    }

    public virtual async Task<WorkspaceDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await StaticStore.GetOrNullAsync(name) ?? await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        var staticWorkspaces = await StaticStore.GetAllAsync();
        var staticWorkspaceNames = staticWorkspaces
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicWorkspaces = await DynamicStore.GetAllAsync();

        return staticWorkspaces.Concat(dynamicWorkspaces.Where(d => !staticWorkspaceNames.Contains(d.Name)))
            .ToImmutableList();
    }
}
