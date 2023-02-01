using Elsa.Services;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks.Tests;

public class PublishWebhookTests : AbpElsaActivitiesWebhooksTests
{
    private readonly IBuildsAndStartsWorkflow _workflowRunner;

    public PublishWebhookTests()
    {
        _workflowRunner = GetRequiredService<IBuildsAndStartsWorkflow>();
    }

    [Fact]
    public async Task Push_Webhook()
    {
        await _workflowRunner.BuildAndStartWorkflowAsync<PublishWebhookWorkflow>();

        MemoryWebhookPublisher.WebHooks.ShouldContainKey(PublishWebhookData.Name);
    }
}
