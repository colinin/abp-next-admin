using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Processor;
using DotNetCore.CAP.Serialization;
using LINGYUN.Abp.EventBus.CAP;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

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
            // 移除默认的定时清理过期消息任务
            // 默认的五分钟,不可配置,时间间隔过短,使用自定义的后台任务
            services.RemoveAll(typeof(CollectorProcessor));
            var capProcessingServiceDescriptor = services
                .FirstOrDefault(x => typeof(CapProcessingServer).Equals(x.ImplementationType));
            if(capProcessingServiceDescriptor != null)
            {
                services.Remove(capProcessingServiceDescriptor);
            }
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IProcessingServer, AbpCapProcessingServer>());
            // 替换为自己的实现
            services.AddSingleton<ISubscribeInvoker, AbpCAPSubscribeInvoker>();
            services.AddSingleton<ISerializer, AbpCapSerializer>();
            return services;
        }
    }
}
