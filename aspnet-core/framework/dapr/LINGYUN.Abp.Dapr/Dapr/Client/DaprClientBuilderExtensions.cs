using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Dapr.Client;

public static class DaprClientBuilderExtensions
{
    public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<DaprClient> configureClient)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(configureClient, nameof(configureClient));

        builder.Services.Configure<DaprClientFactoryOptions>(builder.Name, options =>
        {
            options.DaprClientActions.Add(configureClient);
        });

        return builder;
    }

    public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<DaprClientBuilder> configureBuilder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(configureBuilder, nameof(configureBuilder));

        builder.Services.Configure<DaprClientFactoryOptions>(builder.Name, options =>
        {
            options.DaprClientBuilderActions.Add(configureBuilder);
        });

        return builder;
    }

    public static IDaprClientBuilder ConfigureDaprClient(this IDaprClientBuilder builder, Action<IServiceProvider, DaprClientBuilder> configureClientBuilder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(configureClientBuilder, nameof(configureClientBuilder));

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
