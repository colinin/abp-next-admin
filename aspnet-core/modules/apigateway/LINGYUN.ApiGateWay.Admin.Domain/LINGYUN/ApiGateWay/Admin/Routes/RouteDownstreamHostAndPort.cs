using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteDownstreamHostAndPort : HostAndPort
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteDownstreamHostAndPort()
        {

        }
        public RouteDownstreamHostAndPort(Guid routeId, Guid hostId, string key, int? port)
            : base(hostId, key, port)
        {
            RouteId = routeId;
        }
    }
}
