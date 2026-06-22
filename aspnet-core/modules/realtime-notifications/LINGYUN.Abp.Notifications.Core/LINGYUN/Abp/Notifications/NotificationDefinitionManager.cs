using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinitionManager : INotificationDefinitionManager, ITransientDependency
{
    private readonly AbpNotificationsOptions _notificationsOptions;
    private readonly IStaticNotificationDefinitionStore _staticStore;
    private readonly IDynamicNotificationDefinitionStore _dynamicStore;

    public NotificationDefinitionManager(
        IStaticNotificationDefinitionStore staticStore,
        IDynamicNotificationDefinitionStore dynamicStore,
        IOptions<AbpNotificationsOptions> notificationsOptions)
    {
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
        _notificationsOptions = notificationsOptions.Value;
    }

    public async virtual Task<NotificationDefinition> GetAsync(string name)
    {
        var notification = await GetOrNullAsync(name);
        if (notification == null)
        {
            throw new AbpException("Undefined notification: " + name);
        }

        return notification;
    }

    public async virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await _staticStore.GetOrNullAsync(name) ??
               await _dynamicStore.GetOrNullAsync(name);
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        var staticNotifications = await _staticStore.GetNotificationsAsync();
        var dynamicNotifications = await _dynamicStore.GetNotificationsAsync();

        // 根据策略处理通知定义
        return _notificationsOptions.DynamicNotificationStrategy switch
        {
            DynamicNotificationStrategy.Ignore => await GetNotificationsWithIgnoreStrategy(staticNotifications, dynamicNotifications),
            DynamicNotificationStrategy.Covering => await GetNotificationsWithCoveringStrategy(staticNotifications, dynamicNotifications),
            DynamicNotificationStrategy.Merge => await GetNotificationsWithMergeStrategy(staticNotifications, dynamicNotifications),
            _ => await GetNotificationsWithIgnoreStrategy(staticNotifications, dynamicNotifications)
        };
    }

    public async virtual Task<NotificationGroupDefinition> GetGroupOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await _staticStore.GetGroupOrNullAsync(name) ??
               await _dynamicStore.GetGroupOrNullAsync(name);
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await _staticStore.GetGroupsAsync();
        var dynamicGroups = await _dynamicStore.GetGroupsAsync();

        // 根据策略处理分组定义
        return _notificationsOptions.DynamicNotificationStrategy switch
        {
            DynamicNotificationStrategy.Ignore => await GetGroupsWithIgnoreStrategy(staticGroups, dynamicGroups),
            DynamicNotificationStrategy.Covering => await GetGroupsWithCoveringStrategy(staticGroups, dynamicGroups),
            DynamicNotificationStrategy.Merge => await GetGroupsWithMergeStrategy(staticGroups, dynamicGroups),
            _ => await GetGroupsWithIgnoreStrategy(staticGroups, dynamicGroups)
        };
    }

    #region 通知定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态通知
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsWithIgnoreStrategy(
        IReadOnlyList<NotificationDefinition> staticNotifications,
        IReadOnlyList<NotificationDefinition> dynamicNotifications)
    {
        var staticNotificationNames = staticNotifications
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<NotificationDefinition>>(
            staticNotifications
                .Concat(dynamicNotifications.Where(d => !staticNotificationNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态通知
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsWithCoveringStrategy(
        IReadOnlyList<NotificationDefinition> staticNotifications,
        IReadOnlyList<NotificationDefinition> dynamicNotifications)
    {
        var dynamicNotificationNames = dynamicNotifications
            .Select(p => p.Name)
            .ToImmutableHashSet();

        // 动态通知完全覆盖静态通知
        var result = dynamicNotifications
            .Concat(staticNotifications.Where(s => !dynamicNotificationNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<NotificationDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态通知，创建新实例
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsWithMergeStrategy(
        IReadOnlyList<NotificationDefinition> staticNotifications,
        IReadOnlyList<NotificationDefinition> dynamicNotifications)
    {
        var mergedNotifications = new Dictionary<string, NotificationDefinition>();

        // 先添加所有静态通知
        foreach (var staticNotification in staticNotifications)
        {
            mergedNotifications[staticNotification.Name] = staticNotification;
        }

        // 合并动态通知
        foreach (var dynamicNotification in dynamicNotifications)
        {
            if (mergedNotifications.TryGetValue(dynamicNotification.Name, out var existingNotification))
            {
                // 通知已存在，创建新的合并通知
                var mergedNotification = MergeNotification(existingNotification, dynamicNotification);
                mergedNotifications[dynamicNotification.Name] = mergedNotification;
            }
            else
            {
                // 添加新的动态通知
                mergedNotifications[dynamicNotification.Name] = dynamicNotification;
            }
        }

        return Task.FromResult<IReadOnlyList<NotificationDefinition>>(mergedNotifications.Values.ToImmutableList());
    }

    /// <summary>
    /// 合并两个通知定义，返回新的 NotificationDefinition 实例
    /// </summary>
    protected virtual NotificationDefinition MergeNotification(
        NotificationDefinition staticNotification,
        NotificationDefinition dynamicNotification)
    {
        // 决定使用哪个显示名称（优先使用动态的）
        var displayName = dynamicNotification.DisplayName ?? staticNotification.DisplayName;

        // 决定使用哪个描述（优先使用动态的）
        var description = dynamicNotification.Description ?? staticNotification.Description;

        // 决定通知类型（优先使用动态的）
        var notificationType = dynamicNotification.NotificationType != NotificationType.Application
            ? dynamicNotification.NotificationType
            : staticNotification.NotificationType;

        // 决定存活类型（优先使用动态的）
        var lifetime = dynamicNotification.NotificationLifetime != NotificationLifetime.Persistent
            ? dynamicNotification.NotificationLifetime
            : staticNotification.NotificationLifetime;

        // 决定内容类型（优先使用动态的）
        var contentType = dynamicNotification.ContentType != NotificationContentType.Text
            ? dynamicNotification.ContentType
            : staticNotification.ContentType;

        // 决定是否允许客户端订阅（优先使用动态的）
        var allowSubscriptionToClients = dynamicNotification.AllowSubscriptionToClients || staticNotification.AllowSubscriptionToClients;

        // 创建新的通知实例
        var mergedNotification = new NotificationDefinition(
            staticNotification.Name, // 保持名称不变
            displayName,
            description,
            notificationType,
            lifetime,
            contentType,
            allowSubscriptionToClients
        );

        // 复制静态通知的属性
        foreach (var property in staticNotification.Properties)
        {
            mergedNotification.Properties[property.Key] = property.Value;
        }

        // 复制动态通知的属性（覆盖同名的静态属性）
        foreach (var property in dynamicNotification.Properties)
        {
            mergedNotification.Properties[property.Key] = property.Value;
        }

        // 合并提供者
        foreach (var provider in staticNotification.Providers)
        {
            if (!mergedNotification.Providers.Contains(provider))
            {
                mergedNotification.Providers.Add(provider);
            }
        }

        foreach (var provider in dynamicNotification.Providers)
        {
            if (!mergedNotification.Providers.Contains(provider))
            {
                mergedNotification.Providers.Add(provider);
            }
        }

        // 合并模板（优先使用动态的）
        if (dynamicNotification.Template != null)
        {
            mergedNotification.WithTemplate(dynamicNotification.Template);
        }
        else if (staticNotification.Template != null)
        {
            mergedNotification.WithTemplate(staticNotification.Template);
        }

        return mergedNotification;
    }

    #endregion

    #region 分组定义策略

    /// <summary>
    /// 忽略策略：静态优先，过滤掉同名的动态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsWithIgnoreStrategy(
        IReadOnlyList<NotificationGroupDefinition> staticGroups,
        IReadOnlyList<NotificationGroupDefinition> dynamicGroups)
    {
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<NotificationGroupDefinition>>(
            staticGroups
                .Concat(dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name)))
                .ToImmutableList()
        );
    }

    /// <summary>
    /// 覆盖策略：动态完全覆盖静态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsWithCoveringStrategy(
        IReadOnlyList<NotificationGroupDefinition> staticGroups,
        IReadOnlyList<NotificationGroupDefinition> dynamicGroups)
    {
        var dynamicGroupNames = dynamicGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var result = dynamicGroups
            .Concat(staticGroups.Where(s => !dynamicGroupNames.Contains(s.Name)))
            .ToImmutableList();

        return Task.FromResult<IReadOnlyList<NotificationGroupDefinition>>(result);
    }

    /// <summary>
    /// 合并策略：合并静态和动态分组
    /// </summary>
    protected virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsWithMergeStrategy(
        IReadOnlyList<NotificationGroupDefinition> staticGroups,
        IReadOnlyList<NotificationGroupDefinition> dynamicGroups)
    {
        var mergedGroups = new Dictionary<string, NotificationGroupDefinition>();

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
                // 分组已存在，合并通知
                MergeGroupNotifications(existingGroup, dynamicGroup);
            }
            else
            {
                // 添加新的动态分组
                mergedGroups[dynamicGroup.Name] = dynamicGroup;
            }
        }

        return Task.FromResult<IReadOnlyList<NotificationGroupDefinition>>(
            mergedGroups.Values.ToImmutableList()
        );
    }

    /// <summary>
    /// 合并分组的通知列表
    /// </summary>
    private static void MergeGroupNotifications(NotificationGroupDefinition target, NotificationGroupDefinition source)
    {
        foreach (var sourceNotification in source.Notifications)
        {
            var existingNotification = target.GetNotificationOrNull(sourceNotification.Name);

            if (existingNotification == null)
            {
                // 通知不存在，直接添加
                var newNotification = target.AddNotification(
                    sourceNotification.Name,
                    sourceNotification.DisplayName,
                    sourceNotification.Description,
                    sourceNotification.NotificationType,
                    sourceNotification.NotificationLifetime,
                    sourceNotification.ContentType,
                    sourceNotification.AllowSubscriptionToClients
                );

                // 复制提供者
                foreach (var provider in sourceNotification.Providers)
                {
                    if (!newNotification.Providers.Contains(provider))
                    {
                        newNotification.Providers.Add(provider);
                    }
                }

                // 复制属性
                foreach (var property in sourceNotification.Properties)
                {
                    newNotification.Properties[property.Key] = property.Value;
                }

                // 复制模板
                if (sourceNotification.Template != null)
                {
                    newNotification.WithTemplate(sourceNotification.Template);
                }
            }
            else
            {
                // 通知已存在，合并属性
                foreach (var property in sourceNotification.Properties)
                {
                    existingNotification.Properties[property.Key] = property.Value;
                }

                // 合并提供者
                foreach (var provider in sourceNotification.Providers)
                {
                    if (!existingNotification.Providers.Contains(provider))
                    {
                        existingNotification.Providers.Add(provider);
                    }
                }

                // 更新显示名称（如果源提供了）
                if (sourceNotification.DisplayName != null)
                {
                    existingNotification.DisplayName = sourceNotification.DisplayName;
                }

                // 更新描述（如果源提供了）
                if (sourceNotification.Description != null)
                {
                    existingNotification.Description = sourceNotification.Description;
                }

                // 更新模板（优先使用动态的）
                if (sourceNotification.Template != null)
                {
                    existingNotification.WithTemplate(sourceNotification.Template);
                }

                // 更新允许客户端订阅
                existingNotification.AllowSubscriptionToClients =
                    existingNotification.AllowSubscriptionToClients || sourceNotification.AllowSubscriptionToClients;
            }
        }
    }

    #endregion
}