using System;
using System.Collections.Generic;
using System.Net;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSendRecord : Entity<Guid>, IHasCreationTime, IHasModificationTime
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid WebhookEventId { get; protected set; }

    public virtual Guid WebhookSubscriptionId { get; protected set; }

    public virtual string Response { get; protected set; }

    public virtual HttpStatusCode? ResponseStatusCode { get; set; }

    public virtual string RequestHeaders { get; protected set; }

    public virtual string ResponseHeaders { get; protected set; }

    public virtual bool SendExactSameData { get; set; }

    public virtual DateTime CreationTime { get; set; }

    public virtual DateTime? LastModificationTime { get; set; }

    public virtual WebhookEventRecord WebhookEvent { get; protected set; }

    protected WebhookSendRecord()
    {
    }

    public WebhookSendRecord(
        Guid id,
        Guid eventId,
        Guid subscriptionId,
        Guid? tenantId = null) : base(id)
    {
        WebhookEventId = eventId;
        WebhookSubscriptionId = subscriptionId;

        TenantId = tenantId;
    }

    public void SetResponse(
        string response,
        HttpStatusCode? statusCode = null,
        string responseHeaders = null)
    {
        Response = Check.Length(response, nameof(response), WebhookSendRecordConsts.MaxResponseLength);
        ResponseStatusCode = statusCode;
        ResponseHeaders = Check.Length(responseHeaders, nameof(responseHeaders), WebhookSendRecordConsts.MaxHeadersLength);
    }

    public void SetRequestHeaders(string requestHeaders = null)
    {
        RequestHeaders = Check.Length(requestHeaders, nameof(requestHeaders), WebhookSendRecordConsts.MaxHeadersLength);
    }
}
