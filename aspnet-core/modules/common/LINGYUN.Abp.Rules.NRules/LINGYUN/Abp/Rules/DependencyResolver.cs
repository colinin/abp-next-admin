using Microsoft.Extensions.DependencyInjection;
using NRules.Extensibility;
using System;

namespace LINGYUN.Abp.Rules
{
    public class DependencyResolver : IDependencyResolver
    {
        protected IServiceProvider ServiceProvider { get; }
        public DependencyResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public virtual object Resolve(IResolutionContext context, Type serviceType)
        {
            return ServiceProvider.GetRequiredService(serviceType);
        }
    }
}
