using LINGYUN.Abp.Logging.Serilog.OpenObserve;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace System.Net.Http;

internal static class HttpClientOpenObserveExtensions
{
    private const string HttpClientName = "__Abp_OpenObserve_Client";
    public static IServiceCollection AddOpenObserveHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<OpenObserveClient>(
            HttpClientName,
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<AbpLoggingSerilogOpenObserveOptions>>().Value;

                client.BaseAddress = new Uri(options.Endpoint);
                if (!options.AccessToken.IsNullOrWhiteSpace() && AuthenticationHeaderValue.TryParse(options.AccessToken, out var authenticationHeader))
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }
            });

        return services;
    }
}
