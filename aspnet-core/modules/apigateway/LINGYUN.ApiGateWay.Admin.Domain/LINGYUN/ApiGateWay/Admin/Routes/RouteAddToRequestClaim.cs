using LINGYUN.ApiGateWay.Admin.Security;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteAddToRequestClaim : Claim
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteAddToRequestClaim()
        {

        }
        public RouteAddToRequestClaim(Guid routeId, Guid claimId, string key, string value)
            : base(claimId, key, value)
        {
            RouteId = routeId;
        }
    }
}
