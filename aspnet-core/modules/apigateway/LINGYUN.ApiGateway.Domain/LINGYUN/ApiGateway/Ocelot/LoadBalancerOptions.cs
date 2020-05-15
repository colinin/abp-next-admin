using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class LoadBalancerOptions : Entity<int>
    {
        public virtual long? ItemId { get; private set; }
        public virtual long? ReRouteId { get; private set; }
        /// <summary>
        /// 负载均衡类型
        /// </summary>
        public virtual string Type { get; private set; }
        /// <summary>
        /// 用于Cookie会话密钥
        /// </summary>
        public virtual string Key { get; private set; }
        /// <summary>
        /// 会话阻断时间
        /// </summary>
        public virtual int? Expiry { get; private set; }
        public virtual ReRoute ReRoute { get; private set; }
        public virtual GlobalConfiguration GlobalConfiguration { get; private set; }

        protected LoadBalancerOptions()
        {

        }
        public LoadBalancerOptions(string type, string key, int? expiry)
        {
            ApplyLoadBalancerOptions(type, key, expiry);
        }

        public LoadBalancerOptions SetReRouteId(long rerouteId)
        {
            ReRouteId = rerouteId;
            return this;
        }

        public LoadBalancerOptions SetItemId(long itemId)
        {
            ItemId = itemId;
            return this;
        }

        public void ApplyLoadBalancerOptions(string type, string key, int? expiry)
        {
            Type = type;
            Key = key;
            Expiry = expiry;
        }
    }
}
