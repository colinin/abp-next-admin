using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LINGYUN.Abp.UI.Navigation
{
    public class NavigationDefinitionContext : INavigationDefinitionContext
    {
        protected List<NavigationDefinition> Navigations { get; }
        public NavigationDefinitionContext(List<NavigationDefinition> navigations)
        {
            Navigations = navigations;
        }
        public virtual IReadOnlyList<NavigationDefinition> GetAll()
        {
            return Navigations.ToImmutableList();
        }

        public virtual void Add(params NavigationDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }
            Navigations.AddRange(definitions);
        }
    }
}
