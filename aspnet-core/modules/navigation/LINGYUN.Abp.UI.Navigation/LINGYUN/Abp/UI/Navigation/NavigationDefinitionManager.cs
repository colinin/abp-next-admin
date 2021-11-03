using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.UI.Navigation
{
    public class NavigationDefinitionManager : INavigationDefinitionManager, ISingletonDependency
    {
        protected Lazy<IList<NavigationDefinition>> NavigationDefinitions { get; }

        protected AbpNavigationOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }

        public NavigationDefinitionManager(
            IOptions<AbpNavigationOptions> options,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;

            NavigationDefinitions = new Lazy<IList<NavigationDefinition>>(CreateSettingDefinitions, true);
        }

        public virtual IReadOnlyList<NavigationDefinition> GetAll()
        {
            return NavigationDefinitions.Value.ToImmutableList();
        }

        protected virtual IList<NavigationDefinition> CreateSettingDefinitions()
        {
            var settings = new List<NavigationDefinition>();

            using (var scope = ServiceProvider.CreateScope())
            {
                var providers = Options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as INavigationDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(new NavigationDefinitionContext(settings));
                }
            }

            return settings;
        }
    }
}
