using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IDynamicWebhookDefinitionStoreCache
{
    string CacheStamp { get; set; }
    
    SemaphoreSlim SyncSemaphore { get; }
    
    DateTime? LastCheckTime { get; set; }

    Task FillAsync(
        List<WebhookGroupDefinitionRecord> webhookGroupRecords,
        List<WebhookDefinitionRecord> webhookRecords);

    WebhookDefinition GetWebhookOrNull(string name);
    
    IReadOnlyList<WebhookDefinition> GetWebhooks();

    WebhookGroupDefinition GetWebhookGroupOrNull(string name);

    IReadOnlyList<WebhookGroupDefinition> GetGroups();
}