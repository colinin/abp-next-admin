using System;

namespace LINGYUN.Abp.Rules
{
    public class RulesInitializationContext : IServiceProvider
    {
        public IServiceProvider ServiceProvider { get; }

        internal RulesInitializationContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType) => ServiceProvider.GetService(serviceType);
    }
}
