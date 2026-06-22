using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;

public class AIToolDefinitionManager : IAIToolDefinitionManager, ISingletonDependency
{
    protected readonly AbpAIToolsOptions AIToolOptions;
    protected readonly IStaticAIToolDefinitionStore StaticStore;
    protected readonly IDynamicAIToolDefinitionStore DynamicStore;

    public AIToolDefinitionManager(
        IStaticAIToolDefinitionStore staticStore,
        IDynamicAIToolDefinitionStore dynamicStore,
        IOptions<AbpAIToolsOptions> aiToolOptions)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
        AIToolOptions = aiToolOptions.Value;
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

        return await StaticStore.GetOrNullAsync(name) ??
               await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        var staticAITools = await StaticStore.GetAllAsync();
        var dynamicAITools = await DynamicStore.GetAllAsync();

        // 根据策略处理AI工具定义
        return AIToolOptions.DynamicAItoolStrategy switch
        {
            DynamicAItoolStrategy.Ignore => await GetAIToolsWithIgnoreStrategy(staticAITools, dynamicAITools),
            DynamicAItoolStrategy.Covering => await GetAIToolsWithCoveringStrategy(staticAITools, dynamicAITools),
            DynamicAItoolStrategy.Merge => await GetAIToolsWithMergeStrategy(staticAITools, dynamicAITools),
            _ => await GetAIToolsWithMergeStrategy(staticAITools, dynamicAITools) // 默认使用合并策略
        };
    }

    #region AI工具定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态AI工具
    /// </summary>
    protected virtual Task<IReadOnlyList<AIToolDefinition>> GetAIToolsWithIgnoreStrategy(
        IReadOnlyList<AIToolDefinition> staticAITools,
        IReadOnlyList<AIToolDefinition> dynamicAITools)
    {
        var staticAIToolNames = staticAITools
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<AIToolDefinition>>(
            staticAITools
                .Concat(dynamicAITools.Where(d => !staticAIToolNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态AI工具
    /// </summary>
    protected virtual Task<IReadOnlyList<AIToolDefinition>> GetAIToolsWithCoveringStrategy(
        IReadOnlyList<AIToolDefinition> staticAITools,
        IReadOnlyList<AIToolDefinition> dynamicAITools)
    {
        var dynamicAIToolNames = dynamicAITools
            .Select(p => p.Name)
            .ToImmutableHashSet();

        // 动态AI工具完全覆盖静态AI工具
        var result = dynamicAITools
            .Concat(staticAITools.Where(s => !dynamicAIToolNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<AIToolDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态AI工具，创建新实例
    /// </summary>
    protected virtual Task<IReadOnlyList<AIToolDefinition>> GetAIToolsWithMergeStrategy(
        IReadOnlyList<AIToolDefinition> staticAITools,
        IReadOnlyList<AIToolDefinition> dynamicAITools)
    {
        var mergedAITools = new Dictionary<string, AIToolDefinition>();

        // 先添加所有静态AI工具
        foreach (var staticAITool in staticAITools)
        {
            mergedAITools[staticAITool.Name] = staticAITool;
        }

        // 合并动态AI工具
        foreach (var dynamicAITool in dynamicAITools)
        {
            if (mergedAITools.TryGetValue(dynamicAITool.Name, out var existingAITool))
            {
                // AI工具已存在，创建新的合并AI工具
                var mergedAITool = MergeAITool(existingAITool, dynamicAITool);
                mergedAITools[dynamicAITool.Name] = mergedAITool;
            }
            else
            {
                // 添加新的动态AI工具
                mergedAITools[dynamicAITool.Name] = dynamicAITool;
            }
        }

        // 处理被删除的AI工具
        foreach (var deletedToolName in AIToolOptions.DeletedAITools)
        {
            if (mergedAITools.ContainsKey(deletedToolName))
            {
                mergedAITools.Remove(deletedToolName);
            }
        }

        return Task.FromResult<IReadOnlyList<AIToolDefinition>>(mergedAITools.Values.ToImmutableList());
    }

    /// <summary>
    /// 合并两个AI工具定义，返回新的 AIToolDefinition 实例
    /// </summary>
    protected virtual AIToolDefinition MergeAITool(
        AIToolDefinition staticAITool,
        AIToolDefinition dynamicAITool)
    {
        // 决定使用哪个提供者（优先使用动态的）
        var provider = !string.IsNullOrEmpty(dynamicAITool.Provider)
            ? dynamicAITool.Provider
            : staticAITool.Provider;

        // 决定使用哪个描述（优先使用动态的）
        var description = dynamicAITool.Description ?? staticAITool.Description;

        // 创建新的AI工具实例（Name是只读的）
        var mergedAITool = new AIToolDefinition(
            staticAITool.Name, // 保持名称不变
            provider,
            description
        );

        // 设置是否启用（只要有一方启用，结果就是启用）
        mergedAITool.IsEnabled = staticAITool.IsEnabled || dynamicAITool.IsEnabled;

        // 设置是否为全局工具（只要有一方是全局，结果就是全局）
        mergedAITool.IsGlobal = staticAITool.IsGlobal || dynamicAITool.IsGlobal;

        // 合并状态检查器
        foreach (var checker in staticAITool.StateCheckers)
        {
            if (!mergedAITool.StateCheckers.Contains(checker))
            {
                mergedAITool.StateCheckers.Add(checker);
            }
        }

        foreach (var checker in dynamicAITool.StateCheckers)
        {
            if (!mergedAITool.StateCheckers.Contains(checker))
            {
                mergedAITool.StateCheckers.Add(checker);
            }
        }

        // 合并属性（动态覆盖静态）
        foreach (var property in staticAITool.Properties)
        {
            mergedAITool.Properties[property.Key] = property.Value;
        }

        foreach (var property in dynamicAITool.Properties)
        {
            mergedAITool.Properties[property.Key] = property.Value;
        }

        return mergedAITool;
    }

    #endregion
}