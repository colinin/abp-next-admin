using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceProvider _serviceProvider;
    private readonly IStaticWebhookDefinitionStore _staticStore;
    private readonly IDynamicWebhookDefinitionStore _dynamicStore;

    public WebhookDefinitionManager(
        IServiceProvider serviceProvider,
        IStaticWebhookDefinitionStore staticStore,
        IDynamicWebhookDefinitionStore dynamicStore)
    {
        _serviceProvider = serviceProvider;
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
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
        var staticWebhookNames = staticWebhooks
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicWebhooks = await _dynamicStore.GetWebhooksAsync();

        return staticWebhooks
            .Concat(dynamicWebhooks.Where(d => !staticWebhookNames.Contains(d.Name)))
            .ToImmutableList();
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
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicGroups = await _dynamicStore.GetGroupsAsync();

        return staticGroups
            .Concat(dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name)))
            .ToImmutableList();
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
}
