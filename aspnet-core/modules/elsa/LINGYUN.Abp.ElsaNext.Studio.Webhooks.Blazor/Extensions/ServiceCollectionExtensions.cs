using Elsa.Studio.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebhooksUIHintHandlers(this IServiceCollection services)
    {
        return services.AddUIHintHandler<WebhookPickerHandler>();
    }
}
