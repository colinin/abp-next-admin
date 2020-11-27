using JetBrains.Annotations;
using LINGYUN.ApiGateWay.Admin.Http;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteHttpDelegatingHandler : HttpDelegatingHandler
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteHttpDelegatingHandler()
        {

        }
        public RouteHttpDelegatingHandler(Guid routeId, Guid httpHandlerId, [NotNull] string name)
            : base(httpHandlerId, name)
        {
            RouteId = routeId;
        }
    }
}
