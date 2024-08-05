using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinitionManager : INotificationDefinitionManager, ITransientDependency
{
    private readonly IStaticNotificationDefinitionStore _staticStore;
    private readonly IDynamicNotificationDefinitionStore _dynamicStore;

    public NotificationDefinitionManager(
        IStaticNotificationDefinitionStore staticStore,
        IDynamicNotificationDefinitionStore dynamicStore)
    {
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
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
        var staticNotificationNames = staticNotifications
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicNotifications = await _dynamicStore.GetNotificationsAsync();

        return staticNotifications
            .Concat(dynamicNotifications.Where(d => !staticNotificationNames.Contains(d.Name)))
            .ToImmutableList();
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
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicGroups = await _dynamicStore.GetGroupsAsync();

        return staticGroups
            .Concat(dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name)))
            .ToImmutableList();
    }
}
