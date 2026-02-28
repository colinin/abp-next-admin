using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Serialization;
using LINGYUN.Abp.EventBus.CAP;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// CAP ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures the consistence services for the consistency.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="capAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddCAPEventBus(this IServiceCollection services, Action<CapOptions> capAction)
    {
        services.AddCap(capAction);
        // 替换为自己的实现
        services.AddSingleton<ISubscribeInvoker, AbpCAPSubscribeInvoker>();
        services.AddSingleton<ISerializer, AbpCapSerializer>();

        // 移除默认CAP启动接口
        services.RemoveAll(service =>
        {
            if (service.ServiceType.IsAssignableFrom(typeof(IBootstrapper)))
            {
                return true;
            }
            // 默认Bootstrapper
            if (service.ImplementationType != null &&
                service.ImplementationType.IsAssignableTo(typeof(IBootstrapper)))
            {
                return true;
            }
            // 默认Bootstrapper HostService
            if (service.ImplementationFactory != null &&
                service.ImplementationFactory.Method.ReturnType.IsAssignableTo(typeof(IBootstrapper)))
            {
                return true;
            }

            return false;
        });
        // 使用重写的接口,不使用BackgroundService
        services.AddSingleton<AbpCAPBootstrapper>();
        services.AddSingleton<IBootstrapper>(sp => sp.GetRequiredService<AbpCAPBootstrapper>());

        return services;
    }
}
