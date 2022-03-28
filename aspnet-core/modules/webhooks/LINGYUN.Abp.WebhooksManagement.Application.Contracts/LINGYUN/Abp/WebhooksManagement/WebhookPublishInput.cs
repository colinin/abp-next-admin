using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookPublishInput
{
    [Required]
    [DynamicStringLength(typeof(WebhookEventRecordConsts), nameof(WebhookEventRecordConsts.MaxWebhookNameLength))]
    public string WebhookName { get; set; }

    [Required]
    [DynamicStringLength(typeof(WebhookEventRecordConsts), nameof(WebhookEventRecordConsts.MaxDataLength))]
    public string Data { get; set; }

    public bool SendExactSameData { get; set; }

    public WebhooksHeaderInput Header { get; set; } = new WebhooksHeaderInput();

    public List<Guid?> TenantIds { get; set; } = new List<Guid?>();
}

public class WebhooksHeaderInput
{
    public bool UseOnlyGivenHeaders { get; set; }

    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
