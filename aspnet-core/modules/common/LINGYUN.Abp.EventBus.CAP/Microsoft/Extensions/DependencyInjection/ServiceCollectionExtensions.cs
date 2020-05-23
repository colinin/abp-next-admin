using DotNetCore.CAP;
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
            return services;
        }
    }
}
