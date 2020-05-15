using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateReRouteConfig : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }
        public virtual string ReRouteKey { get; private set; }
        public virtual string Parameter { get; private set; }
        public virtual string JsonPath { get; private set; }

        protected AggregateReRouteConfig()
        {

        }

        internal AggregateReRouteConfig(long reRouteId)
        {
            ReRouteId = reRouteId;
        }

        public void ApplyReRouteConfig(string routeKey, string paramter, string jsonPath)
        {
            ReRouteKey = routeKey;
            Parameter = paramter;
            JsonPath = jsonPath;
        }
    }
}
