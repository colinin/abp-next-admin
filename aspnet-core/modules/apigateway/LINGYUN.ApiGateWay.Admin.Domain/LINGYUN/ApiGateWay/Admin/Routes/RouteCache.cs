using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteCache : Entity<int>
    {
        public virtual int? TtlSeconds { get; private set; }
        public virtual string Region { get; private set; }
        public virtual Guid RouteId { get; private set; }
        public virtual Route Route { get; private set; }
        protected RouteCache() 
        { 
        }
        public RouteCache(Guid routeId, string region = "", int? ttlSeconds = null)
        {
            RouteId = routeId;
            Region = region;
            TtlSeconds = ttlSeconds;
        }
    }
}
