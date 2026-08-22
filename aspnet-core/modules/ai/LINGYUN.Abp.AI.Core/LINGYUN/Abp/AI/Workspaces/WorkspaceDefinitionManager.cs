using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Workspaces;
public class WorkspaceDefinitionManager : IWorkspaceDefinitionManager, ISingletonDependency
{
    protected readonly AbpAICoreOptions Options;
    protected readonly IStaticWorkspaceDefinitionStore StaticStore;
    protected readonly IDynamicWorkspaceDefinitionStore DynamicStore;

    public WorkspaceDefinitionManager(
        IStaticWorkspaceDefinitionStore staticStore,
        IDynamicWorkspaceDefinitionStore dynamicStore,
        IOptions<AbpAICoreOptions> options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
        Options = options.Value;
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

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        if (staticDefinition != null && dynamicDefinition != null)
        {
            return Options.DynamicWorkspaceStrategy switch
            {
                DynamicWorkspaceStrategy.Ignore => staticDefinition,
                DynamicWorkspaceStrategy.Covering => dynamicDefinition,
                DynamicWorkspaceStrategy.Merge => MergeWorkspace(staticDefinition, dynamicDefinition),
                _ => MergeWorkspace(staticDefinition, dynamicDefinition)
            };
        }

        return staticDefinition ?? dynamicDefinition;
    }

    public virtual async Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        var staticWorkspaces = await StaticStore.GetAllAsync();
        var dynamicWorkspaces = await DynamicStore.GetAllAsync();

        // 根据策略处理工作区定义
        return Options.DynamicWorkspaceStrategy switch
        {
            DynamicWorkspaceStrategy.Ignore => await GetWorkspacesWithIgnoreStrategy(staticWorkspaces, dynamicWorkspaces),
            DynamicWorkspaceStrategy.Covering => await GetWorkspacesWithCoveringStrategy(staticWorkspaces, dynamicWorkspaces),
            DynamicWorkspaceStrategy.Merge => await GetWorkspacesWithMergeStrategy(staticWorkspaces, dynamicWorkspaces),
            _ => await GetWorkspacesWithMergeStrategy(staticWorkspaces, dynamicWorkspaces) // 默认使用合并策略
        };
    }

    #region 工作区定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态工作区
    /// </summary>
    protected virtual Task<IReadOnlyList<WorkspaceDefinition>> GetWorkspacesWithIgnoreStrategy(
        IReadOnlyList<WorkspaceDefinition> staticWorkspaces,
        IReadOnlyList<WorkspaceDefinition> dynamicWorkspaces)
    {
        var staticWorkspaceNames = staticWorkspaces
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<WorkspaceDefinition>>(
            staticWorkspaces
                .Concat(dynamicWorkspaces.Where(d => !staticWorkspaceNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态工作区
    /// </summary>
    protected virtual Task<IReadOnlyList<WorkspaceDefinition>> GetWorkspacesWithCoveringStrategy(
        IReadOnlyList<WorkspaceDefinition> staticWorkspaces,
        IReadOnlyList<WorkspaceDefinition> dynamicWorkspaces)
    {
        var dynamicWorkspaceNames = dynamicWorkspaces
            .Select(p => p.Name)
            .ToImmutableHashSet();

        // 动态工作区完全覆盖静态工作区
        var result = dynamicWorkspaces
            .Concat(staticWorkspaces.Where(s => !dynamicWorkspaceNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<WorkspaceDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态工作区，创建新实例
    /// </summary>
    protected virtual Task<IReadOnlyList<WorkspaceDefinition>> GetWorkspacesWithMergeStrategy(
        IReadOnlyList<WorkspaceDefinition> staticWorkspaces,
        IReadOnlyList<WorkspaceDefinition> dynamicWorkspaces)
    {
        var mergedWorkspaces = new Dictionary<string, WorkspaceDefinition>();

        // 先添加所有静态工作区
        foreach (var staticWorkspace in staticWorkspaces)
        {
            mergedWorkspaces[staticWorkspace.Name] = staticWorkspace;
        }

        // 合并动态工作区
        foreach (var dynamicWorkspace in dynamicWorkspaces)
        {
            if (mergedWorkspaces.TryGetValue(dynamicWorkspace.Name, out var existingWorkspace))
            {
                // 工作区已存在，创建新的合并工作区
                var mergedWorkspace = MergeWorkspace(existingWorkspace, dynamicWorkspace);
                mergedWorkspaces[dynamicWorkspace.Name] = mergedWorkspace;
            }
            else
            {
                // 添加新的动态工作区
                mergedWorkspaces[dynamicWorkspace.Name] = dynamicWorkspace;
            }
        }

        // 处理被删除的工作区
        foreach (var deletedWorkspaceName in Options.DeletedWorkspaces)
        {
            if (mergedWorkspaces.ContainsKey(deletedWorkspaceName))
            {
                mergedWorkspaces.Remove(deletedWorkspaceName);
            }
        }

        return Task.FromResult<IReadOnlyList<WorkspaceDefinition>>(mergedWorkspaces.Values.ToImmutableList());
    }

    /// <summary>
    /// 合并两个工作区定义，返回新的 WorkspaceDefinition 实例
    /// </summary>
    protected virtual WorkspaceDefinition MergeWorkspace(
        WorkspaceDefinition staticWorkspace,
        WorkspaceDefinition dynamicWorkspace)
    {
        // 决定使用哪个提供者（优先使用动态的）
        var provider = !string.IsNullOrEmpty(dynamicWorkspace.Provider)
            ? dynamicWorkspace.Provider
            : staticWorkspace.Provider;

        // 决定使用哪个模型名称（优先使用动态的）
        var modelName = !string.IsNullOrEmpty(dynamicWorkspace.ModelName)
            ? dynamicWorkspace.ModelName
            : staticWorkspace.ModelName;

        // 决定使用哪个显示名称（优先使用动态的）
        var displayName = dynamicWorkspace.DisplayName ?? staticWorkspace.DisplayName;

        // 创建新的工作区实例（Name是只读的）
        var mergedWorkspace = new WorkspaceDefinition(
            staticWorkspace.Name, // 保持名称不变
            provider,
            modelName,
            displayName
        );

        // 设置描述（优先使用动态的）
        mergedWorkspace.Description = dynamicWorkspace.Description ?? staticWorkspace.Description;

        // 设置API密钥（优先使用动态的）
        if (!string.IsNullOrEmpty(dynamicWorkspace.ApiKey))
        {
            mergedWorkspace.WithApiKey(dynamicWorkspace.ApiKey!);
        }
        else if (!string.IsNullOrEmpty(staticWorkspace.ApiKey))
        {
            mergedWorkspace.WithApiKey(staticWorkspace.ApiKey!);
        }

        // 设置API基础URL（优先使用动态的）
        if (!string.IsNullOrEmpty(dynamicWorkspace.ApiBaseUrl))
        {
            mergedWorkspace.WithApiBaseUrl(dynamicWorkspace.ApiBaseUrl!);
        }
        else if (!string.IsNullOrEmpty(staticWorkspace.ApiBaseUrl))
        {
            mergedWorkspace.WithApiBaseUrl(staticWorkspace.ApiBaseUrl!);
        }

        // 设置系统提示词（优先使用动态的）
        mergedWorkspace.SystemPrompt = dynamicWorkspace.SystemPrompt ?? staticWorkspace.SystemPrompt;

        // 设置附加系统提示词（优先使用动态的）
        mergedWorkspace.Instructions = dynamicWorkspace.Instructions ?? staticWorkspace.Instructions;

        // 设置温度值（优先使用动态的）
        mergedWorkspace.Temperature = dynamicWorkspace.Temperature ?? staticWorkspace.Temperature;

        // 设置最大输出token数（优先使用动态的）
        mergedWorkspace.MaxOutputTokens = dynamicWorkspace.MaxOutputTokens ?? staticWorkspace.MaxOutputTokens;

        // 设置频率惩罚（优先使用动态的）
        mergedWorkspace.FrequencyPenalty = dynamicWorkspace.FrequencyPenalty ?? staticWorkspace.FrequencyPenalty;

        // 设置存在惩罚（优先使用动态的）
        mergedWorkspace.PresencePenalty = dynamicWorkspace.PresencePenalty ?? staticWorkspace.PresencePenalty;

        // 设置是否启用（只要有一方启用，结果就是启用）
        mergedWorkspace.IsEnabled = staticWorkspace.IsEnabled || dynamicWorkspace.IsEnabled;

        // 合并状态检查器
        foreach (var checker in staticWorkspace.StateCheckers)
        {
            if (!mergedWorkspace.StateCheckers.Contains(checker))
            {
                mergedWorkspace.StateCheckers.Add(checker);
            }
        }

        foreach (var checker in dynamicWorkspace.StateCheckers)
        {
            if (!mergedWorkspace.StateCheckers.Contains(checker))
            {
                mergedWorkspace.StateCheckers.Add(checker);
            }
        }

        // 合并工具列表（去重）
        foreach (var tool in staticWorkspace.Tools)
        {
            if (!mergedWorkspace.Tools.Contains(tool))
            {
                mergedWorkspace.Tools.Add(tool);
            }
        }

        foreach (var tool in dynamicWorkspace.Tools)
        {
            if (!mergedWorkspace.Tools.Contains(tool))
            {
                mergedWorkspace.Tools.Add(tool);
            }
        }

        // 合并属性（动态覆盖静态）
        foreach (var property in staticWorkspace.Properties)
        {
            mergedWorkspace.Properties[property.Key] = property.Value;
        }

        foreach (var property in dynamicWorkspace.Properties)
        {
            mergedWorkspace.Properties[property.Key] = property.Value;
        }

        return mergedWorkspace;
    }

    #endregion
}
