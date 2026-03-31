using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;
public class AIToolDefinitionManager : IAIToolDefinitionManager, ISingletonDependency
{
    protected readonly IStaticAIToolDefinitionStore StaticStore;
    protected readonly IDynamicAIToolDefinitionStore DynamicStore;

    public AIToolDefinitionManager(
        IStaticAIToolDefinitionStore staticStore,
        IDynamicAIToolDefinitionStore dynamicStore)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<AIToolDefinition> GetAsync(string name)
    {
        var workspace = await GetOrNullAsync(name);
        if (workspace == null)
        {
            throw new AbpException("Undefined AITool: " + name);
        }

        return workspace;
    }

    public virtual async Task<AIToolDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await StaticStore.GetOrNullAsync(name) ?? await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        var staticAITools = await StaticStore.GetAllAsync();
        var staticAIToolNames = staticAITools
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicAITools = await DynamicStore.GetAllAsync();

        return staticAITools.Concat(dynamicAITools.Where(d => !staticAIToolNames.Contains(d.Name)))
            .ToImmutableList();
    }
}
