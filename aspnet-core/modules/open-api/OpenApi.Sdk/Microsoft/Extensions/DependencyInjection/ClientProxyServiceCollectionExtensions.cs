using OpenApi;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ClientProxyServiceCollectionExtensions
    {
        public static IServiceCollection AddClientProxy(this IServiceCollection services, string serverUrl)
        {
            services.AddHttpClient("opensdk", options =>
            {
                options.BaseAddress = new Uri(serverUrl);
            });
            services.AddSingleton<IClientProxy, ClientProxy>();

            return services;
        }
    }
}
