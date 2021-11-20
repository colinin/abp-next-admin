using Dapr.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDaprClientExtensions
    {
        #region Add DaprClient Builder

        public static IServiceCollection AddDaprClient(
            [NotNull] this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddLogging();
            services.AddOptions();

            services.TryAddSingleton<DefaultDaprClientFactory>();
            services.TryAddSingleton<IDaprClientFactory>(serviceProvider => serviceProvider.GetRequiredService<DefaultDaprClientFactory>());

            return services;
        }

        public static IDaprClientBuilder AddDaprClient(
            [NotNull] this IServiceCollection services,
            string name)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDaprClient();

            return new DefaultDaprClientBuilder(services, name);
        }

        public static IDaprClientBuilder AddDaprClient(
           [NotNull] this IServiceCollection services,
           string name,
           Action<DaprClientBuilder> configureClient)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configureClient == null)
            {
                throw new ArgumentNullException(nameof(configureClient));
            }

            services.AddDaprClient();

            var builder = new DefaultDaprClientBuilder(services, name);
            builder.ConfigureDaprClient(configureClient);
            return builder;
        }

        public static IDaprClientBuilder AddDaprClient(
            [NotNull] this IServiceCollection services, 
            string name, 
            Action<IServiceProvider, DaprClientBuilder> configureClient)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configureClient == null)
            {
                throw new ArgumentNullException(nameof(configureClient));
            }

            services.AddDaprClient();
            var builder = new DefaultDaprClientBuilder(services, name);
            builder.ConfigureDaprClient(configureClient);
            return builder;
        }


        #endregion
    }
}
