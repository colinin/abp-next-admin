using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Notifications;

[Dependency(ReplaceServices = true)]
public class DynamicNotificationDefinitionStore : IDynamicNotificationDefinitionStore, ITransientDependency
{
    private readonly AbpNotificationsManagementOptions _notificationsManagementOptions;
    private readonly IDynamicNotificationDefinitionCache _dynamicNotificationDefinitionCache;

    public DynamicNotificationDefinitionStore(
        IDynamicNotificationDefinitionCache dynamicNotificationDefinitionCache,
        IOptions<AbpNotificationsManagementOptions> notificationsManagementOptions)
    {
        _dynamicNotificationDefinitionCache = dynamicNotificationDefinitionCache;
        _notificationsManagementOptions = notificationsManagementOptions.Value;
    }

    public async virtual Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        if (!_notificationsManagementOptions.IsDynamicNotificationStoreEnabled)
        {
            return Array.Empty<NotificationGroupDefinition>();
        }
        return await _dynamicNotificationDefinitionCache.GetGroupsAsync();
    }

    public async virtual Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        if (!_notificationsManagementOptions.IsDynamicNotificationStoreEnabled)
        {
            return Array.Empty<NotificationDefinition>();
        }
        return await _dynamicNotificationDefinitionCache.GetNotificationsAsync();
    }

    public async virtual Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        if (!_notificationsManagementOptions.IsDynamicNotificationStoreEnabled)
        {
            return null;
        }
        return await _dynamicNotificationDefinitionCache.GetOrNullAsync(name);
    }
}
