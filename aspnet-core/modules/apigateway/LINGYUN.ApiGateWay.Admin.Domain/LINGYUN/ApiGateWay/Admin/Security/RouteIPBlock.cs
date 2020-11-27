using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Security
{
    public class RouteIPBlock : Entity<int>
    {
        public virtual string Address { get; private set; }
        public virtual Guid SecurityId { get; private set; }
        public virtual RouteSecurity Security { get; private set; }
        protected RouteIPBlock() { }
        public RouteIPBlock(Guid securityId, string address)
        {
            SecurityId = securityId;
            Address = address;
        }
        public override int GetHashCode()
        {
            if (!Address.IsNullOrWhiteSpace())
            {
                return Address.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Address.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is RouteIPBlock iPBlock)
            {
                return iPBlock.Address.Equals(Address);
            }
            return false;
        }
    }
}
