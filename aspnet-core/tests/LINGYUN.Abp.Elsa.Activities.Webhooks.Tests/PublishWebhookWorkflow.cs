using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks.Tests;

public class PublishWebhookWorkflow : IWorkflow
{
    public void Build(IWorkflowBuilder builder)
    {
        builder
            .WriteLine("This a simple workflow with publish webhook.")
            .PublishWebhook(
                setup: activity => 
                    activity.WithWebhooName(PublishWebhookData.Name)
                            .WithWebhookData(PublishWebhookData.SendData)
                            .WithSendExactSameData(true)
                            .WithUseOnlyGivenHeaders(false)
                            .WithHeaders(new Dictionary<string, string>
                            {
                                { "X-With", "Test" }
                            })
                            .WithTenantId(PublishWebhookData.TenantId))
            .WriteLine("Workflow finished.");
    }
}
