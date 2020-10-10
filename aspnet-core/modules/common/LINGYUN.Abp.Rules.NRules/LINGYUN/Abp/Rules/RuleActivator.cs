using Microsoft.Extensions.DependencyInjection;
using NRules.Fluent;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using NRule = NRules.Fluent.Dsl.Rule;

namespace LINGYUN.Abp.Rules
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IRuleActivator))]
    public class RuleActivator : IRuleActivator
    {
        protected IServiceProvider ServiceProvider { get; }
        public RuleActivator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public virtual IEnumerable<NRule> Activate(Type type)
        {
             return (IEnumerable<NRule>)ServiceProvider.GetServices(type);
        }
    }
}
