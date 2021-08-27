using System;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Rules
{
    public class RulesInitializationContext : IServiceProvider, IHasExtraProperties
    {
        public IServiceProvider ServiceProvider { get; }

        public ExtraPropertyDictionary ExtraProperties { get; }

        internal RulesInitializationContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ExtraProperties = new ExtraPropertyDictionary();
        }

        public object GetService(Type serviceType) => ServiceProvider.GetService(serviceType);
    }
}
