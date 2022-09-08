using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;
using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks;

[Action(
    Category = "PublishWebhook", 
    Description = "Sends webhooks to subscriptions.",
    Outcomes = new[] { OutcomeNames.Done })]
public class PublishWebhook : Activity
{
    private readonly IWebhookPublisher _webhookPublisher;

    [ActivityInput(
        Hint = "Unique name of the webhook.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string WebhooName { get; set; }

    [ActivityInput(
            Hint = "Data to send.",
            UIHint = ActivityInputUIHints.MultiLine,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid },
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName
        )]
    public object WebhookData { get; set; }

    [ActivityInput(
       Hint = "If true, It sends the exact same data as the parameter to clients.",
       SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public bool SendExactSameData { get; set; }

    [ActivityInput(
       Hint = "If true, webhook will only contain given headers. If false given headers will be added to predefined headers in subscription.",
       SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public bool UseOnlyGivenHeaders { get; set; }

    [ActivityInput(
            Hint = "That headers will be sent with the webhook.",
            UIHint = ActivityInputUIHints.MultiLine, DefaultSyntax = SyntaxNames.Json,
            SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid },
            Category = PropertyCategories.Advanced
        )]
    public IDictionary<string, string> Headers { get; set; }

    public PublishWebhook(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }


    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        var tenantId = context.GetTenantId();

        await _webhookPublisher.PublishAsync(
            WebhooName,
            WebhookData,
            tenantId,
            SendExactSameData,
            new WebhookHeader
            {
                UseOnlyGivenHeaders = UseOnlyGivenHeaders,
                Headers = Headers
            });

        return Done();
    }
}
