using NRules.Fluent;
using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules.NRules
{
    public class RuleActivator : IRuleActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public RuleActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Rule> Activate(Type type)
        {
            var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
            var rules = _serviceProvider.GetService(collectionType);

            if (rules != null)
            {
                return (IEnumerable<Rule>)rules;
            }

            return ActivateDefault(type);
        }

        private static IEnumerable<Rule> ActivateDefault(Type type)
        {
            yield return (Rule)Activator.CreateInstance(type);
        }
    }
}
