using LINGYUN.Abp.Webhooks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookDefinitionSerializer : IWebhookDefinitionSerializer, ITransientDependency
{
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public WebhookDefinitionSerializer(
        IGuidGenerator guidGenerator,
        ISimpleStateCheckerSerializer stateCheckerSerializer, 
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
        GuidGenerator = guidGenerator;
    }

    public async Task<(WebhookGroupDefinitionRecord[], WebhookDefinitionRecord[])> 
        SerializeAsync(IEnumerable<WebhookGroupDefinition> webhookGroups)
    {
        var webhookGroupRecords = new List<WebhookGroupDefinitionRecord>();
        var webhookRecords = new List<WebhookDefinitionRecord>();

        foreach (var webhookGroup in webhookGroups)
        {
            webhookGroupRecords.Add(await SerializeAsync(webhookGroup));
            
            foreach (var webhook in webhookGroup.Webhooks)
            {
                webhookRecords.Add(await SerializeAsync(webhook, webhookGroup));
            }
        }

        return (webhookGroupRecords.ToArray(), webhookRecords.ToArray());
    }
    
    public Task<WebhookGroupDefinitionRecord> SerializeAsync(WebhookGroupDefinition webhookGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var webhookGroupRecord = new WebhookGroupDefinitionRecord(
                GuidGenerator.Create(),
                webhookGroup.Name,
                LocalizableStringSerializer.Serialize(webhookGroup.DisplayName)
            );

            foreach (var property in webhookGroup.Properties)
            {
                webhookGroupRecord.SetProperty(property.Key, property.Value);
            }
            
            return Task.FromResult(webhookGroupRecord);
        }
    }
    
    public Task<WebhookDefinitionRecord> SerializeAsync(
        WebhookDefinition webhook,
        WebhookGroupDefinition webhookGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var webhookRecord = new WebhookDefinitionRecord(
                GuidGenerator.Create(),
                webhookGroup?.Name,
                webhook.Name,
                LocalizableStringSerializer.Serialize(webhook.DisplayName),
                LocalizableStringSerializer.Serialize(webhook.Description),
                true,
                SerializeRequiredFeatures(webhook.RequiredFeatures)
            );

            foreach (var property in webhook.Properties)
            {
                webhookRecord.SetProperty(property.Key, property.Value);
            }
            
            return Task.FromResult(webhookRecord);
        }
    }
    
    protected virtual string SerializeRequiredFeatures(List<string> requiredFeatures)
    {
        return requiredFeatures.Any()
            ? requiredFeatures.JoinAsString(",")
            : null;
    }
}