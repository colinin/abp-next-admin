using System;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceConnectionExtensions
{
    public static IServiceCollection AddPushPlusClient(
        this IServiceCollection services)
    {
        services.AddHttpClient(
            "_Abp_PushPlus_Client",
            (httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://www.pushplus.plus");
            });

        return services;
    }
}
