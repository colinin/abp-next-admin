using System;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTuiJuheClient(
        this IServiceCollection services)
    {
        services.AddHttpClient(
            "_Abp_TuiJuhe_Client",
            (httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://tui.juhe.cn");
            });

        return services;
    }
}
