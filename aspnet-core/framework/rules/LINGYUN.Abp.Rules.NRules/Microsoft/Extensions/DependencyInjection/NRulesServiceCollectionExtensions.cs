using LINGYUN.Abp.Rules.NRules;
using NRules;
using NRules.Extensibility;
using NRules.Fluent;
using NRules.RuleModel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NRulesServiceCollectionExtensions
    {
        public static IServiceCollection AddNRules(this IServiceCollection services)
        {
            services.AddSingleton<IActionInterceptor, ActionInterceptor>();
            services.AddSingleton<IRuleActivator, RuleActivator>();
            services.AddSingleton<IDependencyResolver, DependencyResolver>();
            services.AddSingleton<IRuleRepository, RuleRepository>();

            services.AddSingleton((serviceProvider) =>
            {
                return serviceProvider.GetRequiredService<IRuleRepository>().Compile();
            });
            services.AddScoped((serviceProvider) =>
            {
                return serviceProvider.GetRequiredService<ISessionFactory>().CreateSession();
            });

            return services;
        }
    }
}
