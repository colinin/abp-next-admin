using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Webhooks.Features;
using System;

namespace LINGYUN.Abp.ElsaNext.Webhooks.Extensions;

public static class ModuleExtensions
{
    public static IModule UseAbpWebhooks(this IModule configuration, Action<WebhooksFeature>? configure = null)
    {
        configuration.Configure(configure);
        return configuration;
    }
}
