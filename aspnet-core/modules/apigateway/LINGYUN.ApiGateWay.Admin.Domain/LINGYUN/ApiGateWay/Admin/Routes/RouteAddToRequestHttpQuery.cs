using LINGYUN.ApiGateWay.Admin.Http;
using System;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteAddToRequestHttpQuery : HttpQuery
    {
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteAddToRequestHttpQuery()
        {

        }
        public RouteAddToRequestHttpQuery(Guid routeId, Guid httpQueryId, string key, string value)
            : base(httpQueryId, key, value)
        {
            RouteId = routeId;
        }
    }
}
