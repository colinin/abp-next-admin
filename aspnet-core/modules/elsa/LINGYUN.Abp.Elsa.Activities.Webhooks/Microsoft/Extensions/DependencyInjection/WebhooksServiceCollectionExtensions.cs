using Elsa.Options;
using LINGYUN.Abp.Elsa.Activities.Webhooks;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebhooksServiceCollectionExtensions
{
    public static ElsaOptionsBuilder AddWebhooksActivities(this ElsaOptionsBuilder options)
    {
        options.AddActivity<PublishWebhook>();

        return options;
    }
}
