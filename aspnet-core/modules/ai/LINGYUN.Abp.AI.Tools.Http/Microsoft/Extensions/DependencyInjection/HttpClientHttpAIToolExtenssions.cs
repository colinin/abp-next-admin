using LINGYUN.Abp.AI.Tools.Http;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Microsoft.Extensions.DependencyInjection;
internal static class HttpClientHttpAIToolExtenssions
{
    private const string HttpAIToolClient = "__AbpAIHttpToolClient";
    public static IServiceCollection AddHttpAIToolClient(this IServiceCollection services)
    {
        var preOptions = services.ExecutePreConfiguredActions<AbpAIToolsHttpOptiions>();

        var clientBuilder = services.AddHttpClient(HttpAIToolClient, (provider, client) =>
        {
            foreach (var clientBuildAction in preOptions.ClientActions)
            {
                clientBuildAction(provider, client);
            }
        }).ConfigurePrimaryHttpMessageHandler(provider =>
        {
            var handler = new HttpClientHandler();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER")))
            {
                handler.UseCookies = false;
            }

            foreach (var handlerAction in preOptions.ClientHandlerActions)
            {
                handlerAction(provider, handler);
            }

            return handler;
        });

        foreach (var clientBuildAction in preOptions.ClientBuildActions)
        {
            clientBuildAction(clientBuilder);
        }

        return services;
    }

    public static HttpClient GetHttpAIToolClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(HttpAIToolClient);
    }
}
