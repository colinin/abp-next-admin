using LINGYUN.Abp.Dynamic.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Webhooks;

public class WebhookDefinitionManager :
    DynamicDefinitionManager<WebhookGroupDefinition, WebhookDefinition>,
    IWebhookDefinitionManager,
    ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected IStaticWebhookDefinitionStore StaticStore { get; }
    protected IDynamicWebhookDefinitionStore DynamicStore { get; }

    public WebhookDefinitionManager(
        IServiceProvider serviceProvider,
       IStaticWebhookDefinitionStore staticStore,
       IDynamicWebhookDefinitionStore dynamicStore,
       IOptions<AbpDynamicDefinitionsOptions> options)
       : base(options)
    {
        ServiceProvider = serviceProvider;
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<WebhookDefinition> GetAsync(string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined webhook: " + name);
    }

    public async virtual Task<WebhookGroupDefinition> GetGroupAsync(string name)
    {
        return await GetGroupOrNullAsync(name) ?? throw new AbpException("Undefined webhook group: " + name);
    }

    public async virtual Task<WebhookGroupDefinition?> GetGroupOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticGroupDefinition = await StaticStore.GetGroupOrNullAsync(name);
        var dynamicGroupDefinition = await DynamicStore.GetGroupOrNullAsync(name);

        return await GetGroupDefinitionAsync(staticGroupDefinition, dynamicGroupDefinition);
    }

    public async virtual Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync()
    {
        var staticGroupDefinitions = await StaticStore.GetGroupsAsync();
        var dynamicGroupDefinitions = await DynamicStore.GetGroupsAsync();

        return await GetGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions);
    }

    public async virtual Task<WebhookDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    public async virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync()
    {
        var staticDefinitions = await StaticStore.GetWebhooksAsync();
        var dynamicDefinitions = await DynamicStore.GetWebhooksAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<bool> IsAvailableAsync(Guid? tenantId, string name)
    {
        if (tenantId == null)
        {
            return true;
        }

        var webhookDefinition = await GetOrNullAsync(name);

        if (webhookDefinition == null)
        {
            return false;
        }

        if (webhookDefinition.RequiredFeatures.Any() == false)
        {
            return true;
        }

        var currentTenant = ServiceProvider.GetRequiredService<ICurrentTenant>();
        var featureChecker = ServiceProvider.GetRequiredService<IFeatureChecker>();
        using (currentTenant.Change(tenantId))
        {
            if (!await featureChecker.IsEnabledAsync(true, webhookDefinition.RequiredFeatures.ToArray()))
            {
                return false;
            }
        }

        return true;
    }

    protected override string GetDefinitionKey(WebhookDefinition definition)
    {
        return definition.Name;
    }

    protected override string GetGroupDefinitionKey(WebhookGroupDefinition groupDefinition)
    {
        return groupDefinition.Name;
    }

    protected override Task<WebhookDefinition> MergeDefinitionAsync(WebhookDefinition targetDefinition, WebhookDefinition sourceDefinition)
    {
        var displayName = sourceDefinition.DisplayName ?? targetDefinition.DisplayName;
        var description = sourceDefinition.Description ?? targetDefinition.Description;

        var mergedWebhook = new WebhookDefinition(
            targetDefinition.Name,
            displayName,
            description
        )
        {
            GroupName =
                !string.IsNullOrWhiteSpace(sourceDefinition.GroupName)
                ? sourceDefinition.GroupName
                : targetDefinition.GroupName
        };

        foreach (var feature in targetDefinition.RequiredFeatures)
        {
            if (!mergedWebhook.RequiredFeatures.Contains(feature))
            {
                mergedWebhook.RequiredFeatures.Add(feature);
            }
        }

        foreach (var feature in sourceDefinition.RequiredFeatures)
        {
            if (!mergedWebhook.RequiredFeatures.Contains(feature))
            {
                mergedWebhook.RequiredFeatures.Add(feature);
            }
        }
        
        foreach (var property in targetDefinition.Properties)
        {
            mergedWebhook.Properties[property.Key] = property.Value;
        }

        foreach (var property in sourceDefinition.Properties)
        {
            mergedWebhook.Properties[property.Key] = property.Value;
        }

        return Task.FromResult(mergedWebhook);
    }

    protected override Task MergeGroupDefinitionAsync(WebhookGroupDefinition targetGroupDefinition, WebhookGroupDefinition sourceGroupDefinition)
    {
        foreach (var sourceWebhook in sourceGroupDefinition.Webhooks)
        {
            var existingWebhook = targetGroupDefinition.GetWebhookOrNull(sourceWebhook.Name);

            if (existingWebhook == null)
            {
                var newWebhook = targetGroupDefinition.AddWebhook(
                    sourceWebhook.Name,
                    sourceWebhook.DisplayName,
                    sourceWebhook.Description
                );
                newWebhook.GroupName = targetGroupDefinition.Name;

                foreach (var feature in sourceWebhook.RequiredFeatures)
                {
                    if (!newWebhook.RequiredFeatures.Contains(feature))
                    {
                        newWebhook.RequiredFeatures.Add(feature);
                    }
                }

                foreach (var property in sourceWebhook.Properties)
                {
                    newWebhook.Properties[property.Key] = property.Value;
                }
            }
            else
            {
                foreach (var property in sourceWebhook.Properties)
                {
                    existingWebhook.Properties[property.Key] = property.Value;
                }

                foreach (var feature in sourceWebhook.RequiredFeatures)
                {
                    if (!existingWebhook.RequiredFeatures.Contains(feature))
                    {
                        existingWebhook.RequiredFeatures.Add(feature);
                    }
                }

                if (sourceWebhook.DisplayName != null)
                {
                    existingWebhook.DisplayName = sourceWebhook.DisplayName;
                }

                if (sourceWebhook.Description != null)
                {
                    existingWebhook.Description = sourceWebhook.Description;
                }

                existingWebhook.GroupName = targetGroupDefinition.Name;
            }
        }
        return Task.CompletedTask;
    }
}
