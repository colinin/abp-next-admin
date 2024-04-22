using System;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWxPusherClient(
        this IServiceCollection services)
    {
        services.AddHttpClient(
            "_Abp_WxPusher_Client",
            (httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://wxpusher.zjiecode.com");
            });

        return services;
    }
}
