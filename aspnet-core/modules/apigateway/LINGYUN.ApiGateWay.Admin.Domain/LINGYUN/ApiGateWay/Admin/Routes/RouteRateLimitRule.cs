using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteRateLimitRule : Entity<int>
    {
        public virtual Guid RateLimitRuleId { get; protected set; }
        public virtual RateLimitRule RateLimitRule { get; protected set; }
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteRateLimitRule()
        {

        }

        public RouteRateLimitRule(Guid routeId, Guid rateLimitRuleId)
        {
            RouteId = routeId;
            RateLimitRuleId = rateLimitRuleId;
        }
    }
}
