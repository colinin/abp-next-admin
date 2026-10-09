using LINGYUN.Abp.OpenObserve;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace System.Net.Http;

internal static class HttpClientOpenObserveExtensions
{
    private const string HttpClientName = "__Abp_OpenObserve_Client";

    public static IServiceCollection AddOpenObserveClient(this IServiceCollection services)
    {
        services.AddHttpClient<OpenObserveClient>(
            HttpClientName,
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<AbpOpenObserveOptions>>().Value;

                client.BaseAddress = new Uri(options.Endpoint, UriKind.Absolute);
                // 搜索接口的 timeout 默认 600 秒, HttpClient 默认 100 秒会把请求中断
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);

                var authenticationHeader = options.CreateAuthorizationHeader();
                if (authenticationHeader != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }
            });

        return services;
    }
}
