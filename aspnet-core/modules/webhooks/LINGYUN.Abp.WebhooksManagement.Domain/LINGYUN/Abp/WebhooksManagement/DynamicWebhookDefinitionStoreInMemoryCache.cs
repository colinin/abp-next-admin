using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.WebhooksManagement;

[ExposeServices(
    typeof(IDynamicWebhookDefinitionStoreCache),
    typeof(DynamicWebhookDefinitionStoreInMemoryCache))]
public class DynamicWebhookDefinitionStoreInMemoryCache :
    IDynamicWebhookDefinitionStoreCache,
    ISingletonDependency
{
    public string CacheStamp { get; set; }
    
    protected IDictionary<string, WebhookGroupDefinition> WebhookGroupDefinitions { get; }
    protected IDictionary<string, WebhookDefinition> WebhookDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);
    
    public DateTime? LastCheckTime { get; set; }

    public DynamicWebhookDefinitionStoreInMemoryCache(
        ISimpleStateCheckerSerializer stateCheckerSerializer, 
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
        
        WebhookGroupDefinitions = new Dictionary<string, WebhookGroupDefinition>();
        WebhookDefinitions = new Dictionary<string, WebhookDefinition>();
    }

    public Task FillAsync(
        List<WebhookGroupDefinitionRecord> webhookGroupRecords,
        List<WebhookDefinitionRecord> webhookRecords)
    {
        WebhookGroupDefinitions.Clear();
        WebhookDefinitions.Clear();
        
        var context = new WebhookDefinitionContext(WebhookGroupDefinitions);
        
        foreach (var webhookGroupRecord in webhookGroupRecords)
        {
            var webhookGroup = context.AddGroup(
                webhookGroupRecord.Name,
                LocalizableStringSerializer.Deserialize(webhookGroupRecord.DisplayName)
            );
            
            WebhookGroupDefinitions[webhookGroup.Name] = webhookGroup;

            foreach (var property in webhookGroupRecord.ExtraProperties)
            {
                webhookGroup[property.Key] = property.Value;
            }

            var webhookRecordsInThisGroup = webhookRecords
                .Where(p => p.GroupName == webhookGroup.Name);

            foreach (var webhookRecord in webhookRecordsInThisGroup)
            {
                AddWebhook(webhookGroup, webhookRecord);
            }
        }

        return Task.CompletedTask;
    }

    public WebhookDefinition GetWebhookOrNull(string name)
    {
        return WebhookDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<WebhookDefinition> GetWebhooks()
    {
        return WebhookDefinitions.Values.ToList();
    }

    public WebhookGroupDefinition GetWebhookGroupOrNull(string name)
    {
        return WebhookGroupDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<WebhookGroupDefinition> GetGroups()
    {
        return WebhookGroupDefinitions.Values.ToList();
    }

    private void AddWebhook(
        WebhookGroupDefinition webhookGroup,
        WebhookDefinitionRecord webhookRecord)
    {
        ILocalizableString description = null;
        if (!webhookRecord.Description.IsNullOrWhiteSpace())
        {
            description = LocalizableStringSerializer.Deserialize(webhookRecord.Description);
        }
        var webhook = webhookGroup.AddWebhook(
            webhookRecord.Name,
            LocalizableStringSerializer.Deserialize(webhookRecord.DisplayName),
            description
        );
        
        WebhookDefinitions[webhook.Name] = webhook;

        if (!webhookRecord.RequiredFeatures.IsNullOrWhiteSpace())
        {
            webhook.RequiredFeatures.AddRange(webhookRecord.RequiredFeatures.Split(','));
        }

        foreach (var property in webhookRecord.ExtraProperties)
        {
            webhook[property.Key] = property.Value;
        }
    }
}