using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Rules.NRules
{
    [DependsOn(
        typeof(AbpRulesModule))]
    public class AbpNRulesModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddDefinitionRules(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddNRules();

            Configure<AbpRulesOptions>(options =>
            {
                options.Contributors.Add<NRulesContributor>();
            });
        }

        private static void AddDefinitionRules(IServiceCollection services)
        {
            var definitionRules = new List<Type>();

            services.OnRegistered(context =>
            {
                if (typeof(RuleBase).IsAssignableFrom(context.ImplementationType))
                {
                    definitionRules.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpNRulesOptions>(options =>
            {
                options.DefinitionRules.AddIfNotContains(definitionRules);
            });
        }
    }
}
