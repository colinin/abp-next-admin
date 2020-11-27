using LINGYUN.ApiGateWay.Admin.Security;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteRequirementClaim : Claim
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteRequirementClaim()
        {

        }
        public RouteRequirementClaim(Guid routeId, Guid claimId, string key, string value)
            : base(claimId, key, value)
        {
            RouteId = routeId;
        }
    }
}
