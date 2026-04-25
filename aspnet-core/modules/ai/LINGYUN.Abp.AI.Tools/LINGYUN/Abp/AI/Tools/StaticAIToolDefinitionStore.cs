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

namespace LINGYUN.Abp.AI.Tools;
public class StaticAIToolDefinitionStore : IStaticAIToolDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAIToolsOptions Options { get; }
    protected IStaticDefinitionCache<AIToolDefinition, Dictionary<string, AIToolDefinition>> DefinitionCache { get; }

    public StaticAIToolDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpAIToolsOptions> options,
        IStaticDefinitionCache<AIToolDefinition, Dictionary<string, AIToolDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        DefinitionCache = definitionCache;
    }

    public virtual async Task<AIToolDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var workspace = await GetOrNullAsync(name);

        if (workspace == null)
        {
            throw new AbpException("Undefined AITool: " + name);
        }

        return workspace;
    }

    public virtual async Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        var defs = await GetAIToolDefinitionsAsync();
        return defs.Values.ToImmutableList();
    }

    public virtual async Task<AIToolDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetAIToolDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    protected virtual async Task<Dictionary<string, AIToolDefinition>> GetAIToolDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreateAIToolDefinitionsAsync);
    }

    protected virtual Task<Dictionary<string, AIToolDefinition>> CreateAIToolDefinitionsAsync()
    {
        var workspaces = new Dictionary<string, AIToolDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IAIToolDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider?.Define(new AIToolDefinitionContext(workspaces));
            }
        }

        return Task.FromResult(workspaces);
    }
}

