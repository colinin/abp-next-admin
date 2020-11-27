using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Security
{
    public class RouteIPWhite : Entity<int>
    {
        public virtual string Address { get; private set; }
        public virtual Guid SecurityId { get; private set; }
        public virtual RouteSecurity Security { get; private set; }

        protected RouteIPWhite() { }
        public RouteIPWhite(Guid securityId, string address)
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
            if(obj == null ||
               Address.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is RouteIPWhite iPWhite)
            {
                return iPWhite.Address.Equals(Address);
            }
            return false;
        }
    }
}
