using LINGYUN.ApiGateWay.Admin.Http;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteUpstreamHttpMethod : HttpMethod
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteUpstreamHttpMethod()
        {

        }

        public RouteUpstreamHttpMethod(Guid routeId, Guid httpMethodId, string method) 
            : base(httpMethodId, method)
        {
            RouteId = routeId;
        }
    }
}
