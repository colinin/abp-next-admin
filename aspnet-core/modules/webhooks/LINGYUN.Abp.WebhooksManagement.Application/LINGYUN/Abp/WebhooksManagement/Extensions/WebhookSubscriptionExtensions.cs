﻿using Newtonsoft.Json;
using System.Linq;

namespace LINGYUN.Abp.WebhooksManagement.Extensions;

public static class WebhookSubscriptionExtensions
{
    public static WebhookSubscriptionDto ToWebhookSubscriptionDto(this WebhookSubscription webhookSubscription)
    {
        return new WebhookSubscriptionDto
        {
            Id = webhookSubscription.Id,
            TenantId = webhookSubscription.TenantId,
            IsActive = webhookSubscription.IsActive,
            Secret = webhookSubscription.Secret,
            WebhookUri = webhookSubscription.WebhookUri,
            Webhooks = webhookSubscription.GetSubscribedWebhooks(),
            Headers = webhookSubscription.GetWebhookHeaders(),
            CreationTime = webhookSubscription.CreationTime,
            CreatorId = webhookSubscription.CreatorId,
            Description = webhookSubscription.Description,
            ConcurrencyStamp = webhookSubscription.ConcurrencyStamp,
            TimeoutDuration = webhookSubscription.TimeoutDuration,
        };
    }

    public static string ToSubscribedWebhooksString(this WebhookSubscriptionCreateOrUpdateInput webhookSubscription)
    {
        if (webhookSubscription.Webhooks.Any())
        {
            return JsonConvert.SerializeObject(webhookSubscription.Webhooks);
        }

        return null;
    }

    public static string ToWebhookHeadersString(this WebhookSubscriptionCreateOrUpdateInput webhookSubscription)
    {
        if (webhookSubscription.Headers.Any())
        {
            return JsonConvert.SerializeObject(webhookSubscription.Headers);
        }

        return null;
    }
}
