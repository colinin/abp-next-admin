using Castle.DynamicProxy;
using Dapr.Client;
using JetBrains.Annotations;
using LINGYUN.Abp.Dapr.Client;
using LINGYUN.Abp.Dapr.Client.DynamicProxying;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicDaprClientProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

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

        #region Add DaprClient Proxies

        public static IServiceCollection AddDaprClientProxies(
            [NotNull] this IServiceCollection services,
            [NotNull] Assembly assembly,
            [NotNull] string remoteServiceConfigurationName = DaprRemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultServices = true)
        {
            Check.NotNull(services, nameof(assembly));

            var serviceTypes = assembly.GetTypes().Where(IsSuitableForDynamicActorProxying).ToArray();

            foreach (var serviceType in serviceTypes)
            {
                services.AddDaprClientProxy(
                    serviceType,
                    remoteServiceConfigurationName,
                    asDefaultServices
                );
            }

            return services;
        }

        public static IServiceCollection AddDaprClientProxy<T>(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = DaprRemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            return services.AddDaprClientProxy(
                typeof(T),
                remoteServiceConfigurationName,
                asDefaultService
            );
        }

        public static IServiceCollection AddDaprClientProxy(
            [NotNull] this IServiceCollection services,
            [NotNull] Type type,
            [NotNull] string remoteServiceConfigurationName = DaprRemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(type, nameof(type));
            Check.NotNullOrWhiteSpace(remoteServiceConfigurationName, nameof(remoteServiceConfigurationName));

            services.AddDaprClientFactory(remoteServiceConfigurationName);

            services.Configure<AbpDaprClientProxyOptions>(options =>
            {
                options.DaprClientProxies[type] = new DynamicDaprClientProxyConfig(type, remoteServiceConfigurationName);
            });

            var interceptorType = typeof(DynamicDaprClientProxyInterceptor<>).MakeGenericType(type);
            services.AddTransient(interceptorType);

            var interceptorAdapterType = typeof(AbpAsyncDeterminationInterceptor<>).MakeGenericType(interceptorType);

            var validationInterceptorAdapterType =
                typeof(AbpAsyncDeterminationInterceptor<>).MakeGenericType(typeof(ValidationInterceptor));

            if (asDefaultService)
            {
                services.AddTransient(
                    type,
                    serviceProvider => ProxyGeneratorInstance
                        .CreateInterfaceProxyWithoutTarget(
                            type,
                            (IInterceptor)serviceProvider.GetRequiredService(validationInterceptorAdapterType),
                            (IInterceptor)serviceProvider.GetRequiredService(interceptorAdapterType)
                        )
                );
            }

            services.AddTransient(
                typeof(IDaprClientProxy<>).MakeGenericType(type),
                serviceProvider =>
                {
                    var service = ProxyGeneratorInstance
                        .CreateInterfaceProxyWithoutTarget(
                            type,
                            (IInterceptor)serviceProvider.GetRequiredService(validationInterceptorAdapterType),
                            (IInterceptor)serviceProvider.GetRequiredService(interceptorAdapterType)
                        );

                    return Activator.CreateInstance(
                        typeof(DaprClientProxy<>).MakeGenericType(type),
                        service
                    );
                });

            return services;
        }

        private static IServiceCollection AddDaprClientFactory(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = DaprRemoteServiceConfigurationDictionary.DefaultName)
        {
            var preOptions = services.ExecutePreConfiguredActions<AbpDaprClientBuilderOptions>();

            if (preOptions.ConfiguredProxyClients.Contains(remoteServiceConfigurationName))
            {
                return services;
            }

            var clientBuilder = services.AddDaprClient(remoteServiceConfigurationName, (IServiceProvider provider, DaprClientBuilder builder) =>
            {
                var options = provider.GetRequiredService<IOptions<AbpDaprRemoteServiceOptions>>().Value;
                builder.UseHttpEndpoint(
                    options.RemoteServices
                        .GetConfigurationOrDefault(remoteServiceConfigurationName).BaseUrl);

                foreach (var clientBuildAction in preOptions.ProxyClientBuildActions)
                {
                    clientBuildAction(remoteServiceConfigurationName, provider, builder);
                }
            });

            clientBuilder.ConfigureDaprClient((client) =>
            {
                foreach (var clientBuildAction in preOptions.ProxyClientActions)
                {
                    clientBuildAction(remoteServiceConfigurationName, client);
                }
            });
            

            services.PreConfigure<AbpDaprClientBuilderOptions>(options =>
            {
                options.ConfiguredProxyClients.Add(remoteServiceConfigurationName);
            });

            return services;
        }

        private static bool IsSuitableForDynamicActorProxying(Type type)
        {
            //TODO: Add option to change type filter

            return type.IsInterface
                && type.IsPublic
                && !type.IsGenericType
                && typeof(IRemoteService).IsAssignableFrom(type);
        }

        #endregion
    }
}
