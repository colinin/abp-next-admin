using System;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Webhooks;

public class AbpWebhooksOptions
{
    public TimeSpan TimeoutDuration { get; set; }

    public int MaxSendAttemptCount { get; set; }

    public bool IsAutomaticSubscriptionDeactivationEnabled { get; set; }

    public int MaxConsecutiveFailCountBeforeDeactivateSubscription { get; set; }

    public ITypeList<WebhookDefinitionProvider> DefinitionProviders { get; }

    public AbpWebhooksOptions()
    {
        TimeoutDuration = TimeSpan.FromSeconds(60);
        MaxSendAttemptCount = 5;
        MaxConsecutiveFailCountBeforeDeactivateSubscription = MaxSendAttemptCount * 3;

        DefinitionProviders = new TypeList<WebhookDefinitionProvider>();
    }
}
