using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin
{
    public class LoadBalancer : Entity<Guid>
    {
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
        protected LoadBalancer()
        {

        }
        public LoadBalancer(Guid id, string type, string key, int? expiry = null)
        {
            Id = id;
            ApplyPolicy(type, key, expiry);
        }

        public void ApplyPolicy(string type, string key, int? expiry = null)
        {
            Type = type;
            Key = key;
            Expiry = expiry;
        }
    }
}
