using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Webhooks;

[Serializable]
public class WebhookHeader
{
    /// <summary>
    /// If true, webhook will only contain given headers. If false given headers will be added to predefined headers in subscription.
    /// Default is false
    /// </summary>
    public bool UseOnlyGivenHeaders { get; set; }
    
    /// <summary>
    /// That headers will be sent with the webhook.
    /// </summary>
    public IDictionary<string, string> Headers { get; set; }

    public WebhookHeader()
    {
        Headers = new Dictionary<string, string>();
    }
}