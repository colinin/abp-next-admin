using NRules.Extensibility;
using System;

namespace LINGYUN.Abp.Rules.NRules
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyResolver(
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
