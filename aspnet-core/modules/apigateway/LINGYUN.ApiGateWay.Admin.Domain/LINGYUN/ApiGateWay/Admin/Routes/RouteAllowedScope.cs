using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteAllowedScope : Entity<int>
    {
        public virtual string Scope { get; protected set; }
        public virtual Guid AuthenticationId { get; protected set; }
        public virtual RouteAuthentication Authentication { get; protected set; }
        protected RouteAllowedScope()
        {

        }

        public RouteAllowedScope(Guid authenticationId, string scope)
        {
            AuthenticationId = authenticationId;
            Scope = scope;
        }

        public override int GetHashCode()
        {
            if (!Scope.IsNullOrWhiteSpace())
            {
                return Scope.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Scope.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is RouteAllowedScope allowedScope)
            {
                return allowedScope.Scope.Equals(Scope);
            }
            return false;
        }
    }
}
