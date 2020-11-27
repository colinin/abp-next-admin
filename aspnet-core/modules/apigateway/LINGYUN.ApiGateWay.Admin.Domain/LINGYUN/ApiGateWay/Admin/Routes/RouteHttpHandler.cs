using LINGYUN.ApiGateWay.Admin.Http;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteHttpHandler : HttpHandler
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteHttpHandler()
        {

        }
        public RouteHttpHandler(Guid id, Guid routeId)
        {
            Id = id;
            RouteId = routeId;
        }
    }
}
