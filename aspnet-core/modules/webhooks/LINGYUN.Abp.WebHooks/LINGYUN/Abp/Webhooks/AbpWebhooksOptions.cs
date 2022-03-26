using System;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Webhooks;

public class AbpWebhooksOptions
{
    /// <summary>
    /// 默认超时时间
    /// </summary>
    public TimeSpan TimeoutDuration { get; set; }
    /// <summary>
    /// 默认最大发送次数
    /// </summary>
    public int MaxSendAttemptCount { get; set; }
    /// <summary>
    /// 是否达到最大连续失败次数时自动取消订阅
    /// </summary>
    public bool IsAutomaticSubscriptionDeactivationEnabled { get; set; }
    /// <summary>
    /// 取消订阅前最大连续失败次数
    /// </summary>
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
