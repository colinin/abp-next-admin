using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Webhooks;

internal class WebhookDefinitionManager : IWebhookDefinitionManager, ISingletonDependency
{
    private readonly AbpWebhooksOptions _webhooksOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStaticWebhookDefinitionStore _staticStore;
    private readonly IDynamicWebhookDefinitionStore _dynamicStore;

    public WebhookDefinitionManager(
        IServiceProvider serviceProvider,
        IStaticWebhookDefinitionStore staticStore,
        IDynamicWebhookDefinitionStore dynamicStore,
        IOptions<AbpWebhooksOptions> webhooksOptions)
    {
        _serviceProvider = serviceProvider;
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
        _webhooksOptions = webhooksOptions.Value;
    }

    public async virtual Task<WebhookDefinition> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await _staticStore.GetOrNullAsync(name) ??
               await _dynamicStore.GetOrNullAsync(name);
    }

    public async virtual Task<WebhookDefinition> GetAsync(string name)
    {
        var webhook = await GetOrNullAsync(name);
        if (webhook == null)
        {
            throw new AbpException("Undefined webhook: " + name);
        }

        return webhook;
    }

    public async virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync()
    {
        var staticWebhooks = await _staticStore.GetWebhooksAsync();
        var dynamicWebhooks = await _dynamicStore.GetWebhooksAsync();

        // 根据策略处理Webhook定义
        return _webhooksOptions.DynamicWebhookStrategy switch
        {
            DynamicWebhookStrategy.Ignore => await GetWebhooksWithIgnoreStrategy(staticWebhooks, dynamicWebhooks),
            DynamicWebhookStrategy.Covering => await GetWebhooksWithCoveringStrategy(staticWebhooks, dynamicWebhooks),
            DynamicWebhookStrategy.Merge => await GetWebhooksWithMergeStrategy(staticWebhooks, dynamicWebhooks),
            _ => await GetWebhooksWithIgnoreStrategy(staticWebhooks, dynamicWebhooks)
        };
    }

    public async virtual Task<WebhookGroupDefinition> GetGroupOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await _staticStore.GetGroupOrNullAsync(name) ??
               await _dynamicStore.GetGroupOrNullAsync(name);
    }

    public async virtual Task<WebhookGroupDefinition> GetGroupAsync(string name)
    {
        var webhookGroup = await GetGroupOrNullAsync(name);
        if (webhookGroup == null)
        {
            throw new AbpException("Undefined webhook group: " + name);
        }

        return webhookGroup;
    }

    public async virtual Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await _staticStore.GetGroupsAsync();
        var dynamicGroups = await _dynamicStore.GetGroupsAsync();

        // 根据策略处理分组定义
        return _webhooksOptions.DynamicWebhookStrategy switch
        {
            DynamicWebhookStrategy.Ignore => await GetGroupsWithIgnoreStrategy(staticGroups, dynamicGroups),
            DynamicWebhookStrategy.Covering => await GetGroupsWithCoveringStrategy(staticGroups, dynamicGroups),
            DynamicWebhookStrategy.Merge => await GetGroupsWithMergeStrategy(staticGroups, dynamicGroups),
            _ => await GetGroupsWithIgnoreStrategy(staticGroups, dynamicGroups)
        };
    }

    public async Task<bool> IsAvailableAsync(Guid? tenantId, string name)
    {
        if (tenantId == null) // host allowed to subscribe all webhooks
        {
            return true;
        }

        var webhookDefinition = await GetOrNullAsync(name);

        if (webhookDefinition == null)
        {
            return false;
        }

        if (webhookDefinition.RequiredFeatures?.Any() == false)
        {
            return true;
        }

        var currentTenant = _serviceProvider.GetRequiredService<ICurrentTenant>();
        var featureChecker = _serviceProvider.GetRequiredService<IFeatureChecker>();
        using (currentTenant.Change(tenantId))
        {
            if (!await featureChecker.IsEnabledAsync(true, webhookDefinition.RequiredFeatures.ToArray()))
            {
                return false;
            }
        }

        return true;
    }

    #region Webhook定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态Webhook
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksWithIgnoreStrategy(
        IReadOnlyList<WebhookDefinition> staticWebhooks,
        IReadOnlyList<WebhookDefinition> dynamicWebhooks)
    {
        var staticWebhookNames = staticWebhooks
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<WebhookDefinition>>(
            staticWebhooks
                .Concat(dynamicWebhooks.Where(d => !staticWebhookNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态Webhook
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksWithCoveringStrategy(
        IReadOnlyList<WebhookDefinition> staticWebhooks,
        IReadOnlyList<WebhookDefinition> dynamicWebhooks)
    {
        var dynamicWebhookNames = dynamicWebhooks
            .Select(p => p.Name)
            .ToImmutableHashSet();

        // 动态Webhook完全覆盖静态Webhook
        var result = dynamicWebhooks
            .Concat(staticWebhooks.Where(s => !dynamicWebhookNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<WebhookDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态Webhook，创建新实例
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksWithMergeStrategy(
        IReadOnlyList<WebhookDefinition> staticWebhooks,
        IReadOnlyList<WebhookDefinition> dynamicWebhooks)
    {
        var mergedWebhooks = new Dictionary<string, WebhookDefinition>();

        // 先添加所有静态Webhook
        foreach (var staticWebhook in staticWebhooks)
        {
            mergedWebhooks[staticWebhook.Name] = staticWebhook;
        }

        // 合并动态Webhook
        foreach (var dynamicWebhook in dynamicWebhooks)
        {
            if (mergedWebhooks.TryGetValue(dynamicWebhook.Name, out var existingWebhook))
            {
                // Webhook已存在，创建新的合并Webhook
                var mergedWebhook = MergeWebhook(existingWebhook, dynamicWebhook);
                mergedWebhooks[dynamicWebhook.Name] = mergedWebhook;
            }
            else
            {
                // 添加新的动态Webhook
                mergedWebhooks[dynamicWebhook.Name] = dynamicWebhook;
            }
        }

        return Task.FromResult<IReadOnlyList<WebhookDefinition>>(mergedWebhooks.Values.ToImmutableList());
    }

    /// <summary>
    /// 合并两个Webhook定义，返回新的 WebhookDefinition 实例
    /// </summary>
    protected virtual WebhookDefinition MergeWebhook(
        WebhookDefinition staticWebhook,
        WebhookDefinition dynamicWebhook)
    {
        // 决定使用哪个显示名称（优先使用动态的）
        var displayName = dynamicWebhook.DisplayName ?? staticWebhook.DisplayName;

        // 决定使用哪个描述（优先使用动态的）
        var description = dynamicWebhook.Description ?? staticWebhook.Description;

        // 创建新的Webhook实例（WebhookDefinition的Name是只读的）
        var mergedWebhook = new WebhookDefinition(
            staticWebhook.Name, // 保持名称不变
            displayName,
            description
        );

        // 设置分组名称（优先使用动态的）
        if (!string.IsNullOrEmpty(dynamicWebhook.GroupName))
        {
            mergedWebhook.GroupName = dynamicWebhook.GroupName;
        }
        else if (!string.IsNullOrEmpty(staticWebhook.GroupName))
        {
            mergedWebhook.GroupName = staticWebhook.GroupName;
        }

        // 合并必需的功能特性
        foreach (var feature in staticWebhook.RequiredFeatures)
        {
            if (!mergedWebhook.RequiredFeatures.Contains(feature))
            {
                mergedWebhook.RequiredFeatures.Add(feature);
            }
        }

        foreach (var feature in dynamicWebhook.RequiredFeatures)
        {
            if (!mergedWebhook.RequiredFeatures.Contains(feature))
            {
                mergedWebhook.RequiredFeatures.Add(feature);
            }
        }

        // 合并属性（动态覆盖静态）
        foreach (var property in staticWebhook.Properties)
        {
            mergedWebhook.Properties[property.Key] = property.Value;
        }

        foreach (var property in dynamicWebhook.Properties)
        {
            mergedWebhook.Properties[property.Key] = property.Value;
        }

        return mergedWebhook;
    }

    #endregion

    #region 分组定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsWithIgnoreStrategy(
        IReadOnlyList<WebhookGroupDefinition> staticGroups,
        IReadOnlyList<WebhookGroupDefinition> dynamicGroups)
    {
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<WebhookGroupDefinition>>(
            staticGroups
                .Concat(dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsWithCoveringStrategy(
        IReadOnlyList<WebhookGroupDefinition> staticGroups,
        IReadOnlyList<WebhookGroupDefinition> dynamicGroups)
    {
        var dynamicGroupNames = dynamicGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var result = dynamicGroups
            .Concat(staticGroups.Where(s => !dynamicGroupNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<WebhookGroupDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsWithMergeStrategy(
        IReadOnlyList<WebhookGroupDefinition> staticGroups,
        IReadOnlyList<WebhookGroupDefinition> dynamicGroups)
    {
        var mergedGroups = new Dictionary<string, WebhookGroupDefinition>();

        // 先添加所有静态分组
        foreach (var staticGroup in staticGroups)
        {
            mergedGroups[staticGroup.Name] = staticGroup;
        }

        // 合并动态分组
        foreach (var dynamicGroup in dynamicGroups)
        {
            if (mergedGroups.TryGetValue(dynamicGroup.Name, out var existingGroup))
            {
                // 分组已存在，合并Webhook
                MergeGroupWebhooks(existingGroup, dynamicGroup);
            }
            else
            {
                // 添加新的动态分组
                mergedGroups[dynamicGroup.Name] = dynamicGroup;
            }
        }

        return Task.FromResult<IReadOnlyList<WebhookGroupDefinition>>(
            mergedGroups.Values.ToImmutableList()
        );
    }

    /// <summary>
    /// 合并分组的Webhook列表
    /// </summary>
    private void MergeGroupWebhooks(WebhookGroupDefinition target, WebhookGroupDefinition source)
    {
        foreach (var sourceWebhook in source.Webhooks)
        {
            var existingWebhook = target.GetWebhookOrNull(sourceWebhook.Name);

            if (existingWebhook == null)
            {
                // Webhook不存在，直接添加
                var newWebhook = target.AddWebhook(
                    sourceWebhook.Name,
                    sourceWebhook.DisplayName,
                    sourceWebhook.Description
                );

                // 设置分组名称
                newWebhook.GroupName = target.Name;

                // 复制必需的功能特性
                foreach (var feature in sourceWebhook.RequiredFeatures)
                {
                    if (!newWebhook.RequiredFeatures.Contains(feature))
                    {
                        newWebhook.RequiredFeatures.Add(feature);
                    }
                }

                // 复制属性
                foreach (var property in sourceWebhook.Properties)
                {
                    newWebhook.Properties[property.Key] = property.Value;
                }
            }
            else
            {
                // Webhook已存在，合并属性
                foreach (var property in sourceWebhook.Properties)
                {
                    existingWebhook.Properties[property.Key] = property.Value;
                }

                // 合并必需的功能特性
                foreach (var feature in sourceWebhook.RequiredFeatures)
                {
                    if (!existingWebhook.RequiredFeatures.Contains(feature))
                    {
                        existingWebhook.RequiredFeatures.Add(feature);
                    }
                }

                // 更新显示名称（如果源提供了）
                if (sourceWebhook.DisplayName != null)
                {
                    existingWebhook.DisplayName = sourceWebhook.DisplayName;
                }

                // 更新描述（如果源提供了）
                if (sourceWebhook.Description != null)
                {
                    existingWebhook.Description = sourceWebhook.Description;
                }

                // 更新分组名称（确保保持一致）
                existingWebhook.GroupName = target.Name;
            }
        }
    }

    #endregion
}