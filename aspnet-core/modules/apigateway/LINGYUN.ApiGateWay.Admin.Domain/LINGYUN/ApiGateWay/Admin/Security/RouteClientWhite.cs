using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Security
{
    public class RouteClientWhite : Entity<int>
    {
        public virtual string ClientId { get; private set; }
        public virtual Guid RateLimitRuleId { get; private set; }
        public virtual RateLimitRule RateLimitRule { get; private set; }
        public override int GetHashCode()
        {
            if (!ClientId.IsNullOrWhiteSpace())
            {
                return ClientId.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               ClientId.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is RouteClientWhite clientWhite)
            {
                return clientWhite.ClientId.Equals(ClientId);
            }
            return false;
        }
    }
}
