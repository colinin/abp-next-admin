using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Modularity;
using NRule = NRules.Fluent.Dsl.Rule;

namespace LINGYUN.Abp.Rules.NRules
{
    [DependsOn(
        typeof(AbpRulesModule)
        )]
    public class AbpNRulesModule : AbpModule
    {
        private readonly AbpNRulesOptions options = new AbpNRulesOptions();
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddObjectAccessor(options);
            context.Services.OnRegistred(ctx =>
            {
                if (ctx.ImplementationType.IsAssignableTo(typeof(NRule)))
                {
                    options.Rules.AddIfNotContains(ctx.ImplementationType);
                }
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddNRules(options);
        }
    }
}
