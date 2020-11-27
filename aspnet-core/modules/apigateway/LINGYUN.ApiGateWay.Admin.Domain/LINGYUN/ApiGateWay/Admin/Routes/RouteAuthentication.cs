using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteAuthentication : Entity<Guid>
    {
        public virtual string AuthenticationProviderKey { get; private set; }
        public virtual ICollection<RouteAllowedScope> AllowedScopes { get; set; }
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteAuthentication()
        {
            AllowedScopes = new List<RouteAllowedScope>();
        }

        public RouteAuthentication(Guid id, Guid routeId, string providerKey)
        {
            Id = id;
            RouteId = routeId;
            ChangeProvider(providerKey);
        }

        public void ChangeProvider(string providerKey)
        {
            AuthenticationProviderKey = providerKey;
        }

        public void AddScope(string scope)
        {
            AllowedScopes.AddIfNotContains(new RouteAllowedScope(Id, scope));
        }

        public void AddScopes(IEnumerable<string> scope)
        {
            foreach(var scop in scope)
            {
                AddScope(scop);
            }
        }

        public void RemoveScope(string scope)
        {
            AllowedScopes.RemoveAll(x => x.Scope.Equals(scope, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllScope()
        {
            AllowedScopes.Clear();
        }
    }
}
