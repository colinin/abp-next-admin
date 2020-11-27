using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteLoadBalancer : Entity<int>
    {
        public virtual Guid LoadBalancerId { get; private set; }
        public virtual LoadBalancer LoadBalancer { get; private set; }
        public virtual Guid RouteId { get; private set; }
        public virtual Route Route { get; private set; }
        protected RouteLoadBalancer()
        {

        }

        public RouteLoadBalancer(Guid routeId, Guid loadBalancerId)
        {
            RouteId = routeId;
            LoadBalancerId = loadBalancerId;
        }
    }
}
