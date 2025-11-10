using LINGYUN.Abp.WeChat.Work;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;
public static class IHttpClientFactoryWeChatWorkExtensions
{
    internal static IServiceCollection AddApiClient(this IServiceCollection services, Action<HttpClient> configureClient = null)
    {
        services.AddHttpClient(AbpWeChatWorkGlobalConsts.ApiClient,
            options =>
            {
                options.BaseAddress = new Uri("https://qyapi.weixin.qq.com");

                configureClient?.Invoke(options);
            });

        return services;
    }

    public static HttpClient CreateWeChatWorkApiClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);
    }

    internal static IServiceCollection AddOAuthClient(this IServiceCollection services, Action<HttpClient> configureClient = null)
    {
        services.AddHttpClient(AbpWeChatWorkGlobalConsts.OAuthClient,
            options =>
            {
                options.BaseAddress = new Uri("https://open.weixin.qq.com");

                configureClient?.Invoke(options);
            });

        return services;
    }

    public static HttpClient CreateWeChatWorkOAuthClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.OAuthClient);
    }

    internal static IServiceCollection AddLoginClient(this IServiceCollection services, Action<HttpClient> configureClient = null)
    {
        services.AddHttpClient(AbpWeChatWorkGlobalConsts.LoginClient,
            options =>
            {
                options.BaseAddress = new Uri("https://login.work.weixin.qq.com");

                configureClient?.Invoke(options);
            });

        return services;
    }

    public static HttpClient CreateWeChatWorkLoginClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.LoginClient);
    }
}
