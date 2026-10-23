using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Elsa.Workflows.UIHints;
using LINGYUN.Abp.ElsaNext.Webhooks.UIHints;
using LINGYUN.Abp.Webhooks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Webhooks.Activities;

[Activity("Elsa", "Webhooks", "Send webhooks to subscriptions.", Kind = ActivityKind.Task)]
public class PublishWebhook : CodeActivity
{
    [Input(
        Description = "Unique name of the webhook.",
        UIHint = WebhooksUIHints.WebhookPicker)]
    public Input<string> WebhookName { get; set; } = default!;

    [Input(Description = "Data to send.")]
    public Input<object> WebhookData { get; set; } = default!;

    [Input(Description = "If true, It sends the exact same data as the parameter to clients.")]
    public Input<bool?> SendExactSameData { get; set; } = default!;

    [Input(Description = "If true, webhook will only contain given headers. If false given headers will be added to predefined headers in subscription.")]
    public Input<bool?> UseOnlyGivenHeaders { get; set; } = default!;

    [Input(
        Description = "That headers will be sent with the webhook.",
        UIHint = InputUIHints.Dictionary)]
    public Input<Dictionary<string, string>> Headers { get; set; } = default!;

    protected async override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var webhookPublisher = context.GetRequiredService<IWebhookPublisher>();

        var webhookName = WebhookName.Get(context);
        var webhookData = WebhookData.Get(context);
        var sendExactSameData = SendExactSameData.GetOrDefault(context) ?? true;
        WebhookHeader? webhookHeader = null;
        var headers = Headers.GetOrDefault(context);
        if (headers != null)
        {
            var useOnlyGivenHeaders = UseOnlyGivenHeaders.GetOrDefault(context) ?? false;
            webhookHeader = new WebhookHeader
            {
                UseOnlyGivenHeaders = useOnlyGivenHeaders,
                Headers = headers,
            };
        }

        await webhookPublisher.PublishAsync(
            webhookName,
            webhookData,
            sendExactSameData,
            webhookHeader);
    }
}
