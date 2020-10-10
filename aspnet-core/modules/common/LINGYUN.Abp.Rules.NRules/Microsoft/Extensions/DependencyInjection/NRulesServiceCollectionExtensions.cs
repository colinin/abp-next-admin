using LINGYUN.Abp.Rules;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NRulesServiceCollectionExtensions
    {
        public static IServiceCollection AddNRules(this IServiceCollection services, AbpNRulesOptions options)
        {
            services.RegisterRepository();
            services.RegisterSessionFactory();
            services.RegisterSession();

            return services;
        }

        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddSingleton<IRuleActivator, RuleActivator>();

            services.AddSingleton<IRuleRepository, AbpRuleRepository>();
            services.AddSingleton<INRulesRepository, AbpRuleRepository>();

            return services;
        }

        public static IServiceCollection RegisterSessionFactory(this IServiceCollection services)
        {
            services.RegisterSessionFactory((provider) => provider.GetRequiredService<IRuleRepository>().Compile());

            return services;
        }

        public static IServiceCollection RegisterSessionFactory(this IServiceCollection services, Func<IServiceProvider, ISessionFactory> compileFunc)
        {
            services.AddSingleton<IRuleActivator, RuleActivator>();
            services.AddSingleton(compileFunc);

            return services;
        }

        public static IServiceCollection RegisterSession(this IServiceCollection services)
        {
            return services.RegisterSession((provider) => provider.GetRequiredService<ISessionFactory>().CreateSession());
        }

        public static IServiceCollection RegisterSession(this IServiceCollection services, Func<IServiceProvider, ISession> factoryFunc)
        {
            return services.AddScoped(factoryFunc);
        }
    }
}
