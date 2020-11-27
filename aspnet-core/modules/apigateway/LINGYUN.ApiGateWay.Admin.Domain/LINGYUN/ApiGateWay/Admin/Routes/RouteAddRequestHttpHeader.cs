using LINGYUN.ApiGateWay.Admin.Http;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteAddRequestHttpHeader : HttpHeader
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteAddRequestHttpHeader()
        {

        }
        public RouteAddRequestHttpHeader(Guid routeId, Guid httpHeaderId, string key, string value)
            : base(httpHeaderId, key, value)
        {
            RouteId = routeId;
        }
    }
}
