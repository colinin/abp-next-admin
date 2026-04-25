using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;
internal static class TencentHttpClientFactoryServiceCollectionExtensions
{
    private const string HttpClientName = "__BlobManagement_Tencent_Client";
    public static IServiceCollection AddTencentHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName);

        return services;
    }

    public static HttpClient CreateTencentHttpClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(HttpClientName);
    }
}
