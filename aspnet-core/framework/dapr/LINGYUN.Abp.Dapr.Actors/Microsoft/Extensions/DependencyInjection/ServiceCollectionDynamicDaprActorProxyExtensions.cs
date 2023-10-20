using Castle.DynamicProxy;
using Dapr.Actors;
using JetBrains.Annotations;
using LINGYUN.Abp.Dapr.Actors;
using LINGYUN.Abp.Dapr.Actors.DynamicProxying;
using System;
using System.Linq;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDynamicDaprActorProxyExtensions
    {
        private static readonly ProxyGenerator ProxyGeneratorInstance = new ProxyGenerator();

        public static IServiceCollection AddDaprActorProxies(
            [NotNull] this IServiceCollection services,
            [NotNull] Assembly assembly,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultServices = true)
        {
            Check.NotNull(services, nameof(assembly));

            var serviceTypes = assembly.GetTypes().Where(IsSuitableForDynamicActorProxying).ToArray();

            foreach (var serviceType in serviceTypes)
            {
                services.AddDaprActorProxy(
                    serviceType,
                    remoteServiceConfigurationName,
                    asDefaultServices
                );
            }

            return services;
        }

        public static IServiceCollection AddDaprActorProxy<T>(
            [NotNull] this IServiceCollection services,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            return services.AddDaprActorProxy(
                typeof(T),
                remoteServiceConfigurationName,
                asDefaultService
            );
        }

        public static IServiceCollection AddDaprActorProxy(
            [NotNull] this IServiceCollection services,
            [NotNull] Type type,
            [NotNull] string remoteServiceConfigurationName = RemoteServiceConfigurationDictionary.DefaultName,
            bool asDefaultService = true)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(type, nameof(type));
            Check.NotNullOrWhiteSpace(remoteServiceConfigurationName, nameof(remoteServiceConfigurationName));

            // AddHttpClientFactory(services, remoteServiceConfigurationName);

            services.Configure<AbpDaprActorProxyOptions>(options =>
            {
                options.ActorProxies[type] = new DynamicDaprActorProxyConfig(type, remoteServiceConfigurationName);
            });

            var interceptorType = typeof(DynamicDaprActorProxyInterceptor<>).MakeGenericType(type);
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

            return services;
        }

        private static bool IsSuitableForDynamicActorProxying(Type type)
        {
            //TODO: Add option to change type filter

            return type.IsInterface
                && type.IsPublic
                && !type.IsGenericType
                && typeof(IActor).IsAssignableFrom(type);
        }
    }
}
