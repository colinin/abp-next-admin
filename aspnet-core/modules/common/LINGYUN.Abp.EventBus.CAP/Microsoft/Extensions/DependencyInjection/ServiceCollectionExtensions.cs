using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Serialization;
using LINGYUN.Abp.EventBus.CAP;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
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
            return services;
        }
    }
}
