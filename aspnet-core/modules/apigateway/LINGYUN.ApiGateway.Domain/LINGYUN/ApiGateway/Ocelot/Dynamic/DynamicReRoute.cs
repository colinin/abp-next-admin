using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class DynamicReRoute : AggregateRoot<int>
    {
        public virtual long DynamicReRouteId { get; private set; }
        public virtual string ServiceName { get; private set; }
        public virtual string DownstreamHttpVersion { get; private set; }
        public virtual RateLimitRule RateLimitRule { get; private set; }
        public virtual string AppId { get; protected set; }
        protected DynamicReRoute()
        {

        }
        public DynamicReRoute(long dynamicReRouteId, string appId, string serviceName, string downHttpVersion = "")
        {
            AppId = appId;
            ServiceName = serviceName;
            DynamicReRouteId = dynamicReRouteId;
            DownstreamHttpVersion = downHttpVersion;
            RateLimitRule = new RateLimitRule("", null, null);
            RateLimitRule.SetDynamicReRouteId(DynamicReRouteId);
        }

        public void SetRateLimitRule(RateLimitRule limitRule)
        {
            RateLimitRule = limitRule;
        }
    }
}
