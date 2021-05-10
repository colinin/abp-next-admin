using NRules.Extensibility;
using System;

namespace LINGYUN.Abp.Rules.NRules
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDependencyResolver(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(IResolutionContext context, Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
