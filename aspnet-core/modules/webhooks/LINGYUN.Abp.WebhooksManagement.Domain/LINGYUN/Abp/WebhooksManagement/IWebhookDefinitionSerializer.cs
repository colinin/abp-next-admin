using JetBrains.Annotations;
using LINGYUN.Abp.Webhooks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookDefinitionSerializer
{
    Task<(WebhookGroupDefinitionRecord[], WebhookDefinitionRecord[])>
        SerializeAsync(IEnumerable<WebhookGroupDefinition> WebhookGroups);

    Task<WebhookGroupDefinitionRecord> SerializeAsync(
        WebhookGroupDefinition WebhookGroup);

    Task<WebhookDefinitionRecord> SerializeAsync(
        WebhookDefinition Webhook,
        [CanBeNull] WebhookGroupDefinition WebhookGroup);
}