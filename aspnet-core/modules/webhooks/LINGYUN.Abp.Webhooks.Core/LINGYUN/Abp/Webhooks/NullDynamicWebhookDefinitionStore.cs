using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks;

[Dependency(TryRegister = true)]
public class NullDynamicWebhookDefinitionStore : IDynamicWebhookDefinitionStore, ISingletonDependency
{
    private readonly static Task<WebhookDefinition> CachedWebhookResult = Task.FromResult((WebhookDefinition)null);

    private readonly static Task<WebhookGroupDefinition> CachedWebhookGroupResult = Task.FromResult((WebhookGroupDefinition)null);

    private readonly static Task<IReadOnlyList<WebhookDefinition>> CachedWebhooksResult =
        Task.FromResult((IReadOnlyList<WebhookDefinition>)Array.Empty<WebhookDefinition>().ToImmutableList());

    private readonly static Task<IReadOnlyList<WebhookGroupDefinition>> CachedGroupsResult =
        Task.FromResult((IReadOnlyList<WebhookGroupDefinition>)Array.Empty<WebhookGroupDefinition>().ToImmutableList());

    public Task<WebhookDefinition> GetOrNullAsync(string name)
    {
        return CachedWebhookResult;
    }

    public Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync()
    {
        return CachedWebhooksResult;
    }

    public Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync()
    {
        return CachedGroupsResult;
    }

    public Task<WebhookGroupDefinition> GetGroupOrNullAsync(string name)
    {
        return CachedWebhookGroupResult;
    }
}
