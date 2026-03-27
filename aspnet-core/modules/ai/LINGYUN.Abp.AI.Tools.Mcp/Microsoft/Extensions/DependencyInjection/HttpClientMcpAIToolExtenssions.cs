using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection;
internal static class HttpClientMcpAIToolExtenssions
{
    private const string McpAIToolClient = "__AbpAIMcpToolClient";
    public static IServiceCollection AddMcpAIToolClient(this IServiceCollection services)
    {
        services.AddHttpClient(McpAIToolClient);

        return services;
    }

    public static HttpClient CreateMcpAIToolClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(McpAIToolClient);
    }
}
