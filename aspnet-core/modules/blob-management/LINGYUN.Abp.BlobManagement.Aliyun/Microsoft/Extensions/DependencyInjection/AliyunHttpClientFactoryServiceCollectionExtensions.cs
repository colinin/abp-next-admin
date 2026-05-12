using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;
internal static class AliyunHttpClientFactoryServiceCollectionExtensions
{
    private const string HttpClientName = "__BlobManagement_Aliyun_Client";
    public static IServiceCollection AddAliyunHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName);

        return services;
    }

    public static HttpClient CreateAliyunHttpClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(HttpClientName);
    }
}
