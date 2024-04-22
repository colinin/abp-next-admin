using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

[Dependency(TryRegister = true)]
public class NullDynamicNotificationDefinitionStore : IDynamicNotificationDefinitionStore, ISingletonDependency
{
    private readonly static Task<NotificationDefinition> CachedNotificationResult = Task.FromResult((NotificationDefinition)null);
    private readonly static Task<NotificationGroupDefinition> CachedNotificationGroupResult = Task.FromResult((NotificationGroupDefinition)null);

    private readonly static Task<IReadOnlyList<NotificationDefinition>> CachedNotificationsResult =
        Task.FromResult((IReadOnlyList<NotificationDefinition>)Array.Empty<NotificationDefinition>().ToImmutableList());

    private readonly static Task<IReadOnlyList<NotificationGroupDefinition>> CachedGroupsResult =
        Task.FromResult((IReadOnlyList<NotificationGroupDefinition>)Array.Empty<NotificationGroupDefinition>().ToImmutableList());

    public Task<NotificationDefinition> GetOrNullAsync(string name)
    {
        return CachedNotificationResult;
    }

    public Task<IReadOnlyList<NotificationDefinition>> GetNotificationsAsync()
    {
        return CachedNotificationsResult;
    }

    public Task<NotificationGroupDefinition> GetGroupOrNullAsync(string name)
    {
        return CachedNotificationGroupResult;
    }

    public Task<IReadOnlyList<NotificationGroupDefinition>> GetGroupsAsync()
    {
        return CachedGroupsResult;
    }
}
