using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class CacheOptions : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }
        public virtual int? TtlSeconds { get; private set; }
        public virtual string Region { get; private set; }
        public virtual ReRoute ReRoute { get; private set; }
        protected CacheOptions()
        {

        }
        public CacheOptions(long rerouteId)
        {
            ReRouteId = rerouteId;
        }

        public void ApplyCacheOption(int? ttl, string region)
        {
            TtlSeconds = ttl;
            Region = region;
        }
    }
}
