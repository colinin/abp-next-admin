using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Dapr.Client
{
    public static class DaprClientBuilderExtensions
    {
        public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<DaprClient> configureClient)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureClient == null)
            {
                throw new ArgumentNullException(nameof(configureClient));
            }

            builder.Services.Configure<DaprClientFactoryOptions>(builder.Name, options =>
            {
                options.DaprClientActions.Add(configureClient);
            });

            return builder;
        }

        public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<DaprClientBuilder> configureBuilder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureBuilder));
            }

            builder.Services.Configure<DaprClientFactoryOptions>(builder.Name, options =>
            {
                options.DaprClientBuilderActions.Add(configureBuilder);
            });

            return builder;
        }

        public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<IServiceProvider, DaprClientBuilder> configureClientBuilder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureClientBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureClientBuilder));
            }

            builder.Services.AddTransient<IConfigureOptions<DaprClientFactoryOptions>>(services =>
            {
                return new ConfigureNamedOptions<DaprClientFactoryOptions>(builder.Name, (options) =>
                {
                    options.DaprClientBuilderActions.Add(client => configureClientBuilder(services, client));
                });
            });

            return builder;
        }
    }
}
