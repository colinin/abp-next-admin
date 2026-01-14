using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.StaticDefinitions;

namespace LINGYUN.Abp.AI.Workspaces;
public class StaticWorkspaceDefinitionStore : IStaticWorkspaceDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAICoreOptions Options { get; }
    protected IStaticDefinitionCache<WorkspaceDefinition, Dictionary<string, WorkspaceDefinition>> DefinitionCache { get; }

    public StaticWorkspaceDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpAICoreOptions> options,
        IStaticDefinitionCache<WorkspaceDefinition, Dictionary<string, WorkspaceDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        DefinitionCache = definitionCache;
    }

    public virtual async Task<WorkspaceDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var workspace = await GetOrNullAsync(name);

        if (workspace == null)
        {
            throw new AbpException("Undefined workspace: " + name);
        }

        return workspace;
    }

    public virtual async Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        var defs = await GetWorkspaceDefinitionsAsync();
        return defs.Values.ToImmutableList();
    }

    public virtual async Task<WorkspaceDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetWorkspaceDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    protected virtual async Task<Dictionary<string, WorkspaceDefinition>> GetWorkspaceDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreateWorkspaceDefinitionsAsync);
    }

    protected virtual Task<Dictionary<string, WorkspaceDefinition>> CreateWorkspaceDefinitionsAsync()
    {
        var workspaces = new Dictionary<string, WorkspaceDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IWorkspaceDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider?.Define(new WorkspaceDefinitionContext(workspaces));
            }
        }

        return Task.FromResult(workspaces);
    }
}
