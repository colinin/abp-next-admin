using AutoMapper;
using LINGYUN.Abp.Webhooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhooksManagementDomainMapperProfile : Profile
{
    public WebhooksManagementDomainMapperProfile()
    {
        CreateMap<WebhookEventRecord, WebhookEventEto>();
        CreateMap<WebhookSendRecord, WebhookSendAttemptEto>();
        CreateMap<WebhookSubscription, WebhookSubscriptionEto>();

        CreateMap<WebhookEventRecord, WebhookEvent>();
        CreateMap<WebhookSendRecord, WebhookSendAttempt>();
    }
}
