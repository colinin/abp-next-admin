using LINGYUN.ApiGateWay.Admin.Routes;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Security
{
    public class RouteSecurity : Entity<Guid>
    {
        public virtual ICollection<RouteIPWhite> IPAllowedList { get; private set; }
        public virtual ICollection<RouteIPBlock> IPBlockedList { get; private set; }
        public virtual Guid RouteId { get; private set; }
        public virtual Route Route { get; private set; }
        protected RouteSecurity()
        {
            IPAllowedList = new List<RouteIPWhite>();
            IPBlockedList = new List<RouteIPBlock>();
        }

        public RouteSecurity(Guid id, Guid routeId) : this()
        {
            Id = id;
            RouteId = routeId;
        }

        public void AddIpWhite(string address)
        {
            IPAllowedList.AddIfNotContains(new RouteIPWhite(Id, address));
        }

        public void RemoveIpWhite(string address)
        {
            IPAllowedList.RemoveAll(ip => ip.Address.Equals(address));
        }

        public void RemoveAllIpWhite()
        {
            IPAllowedList.Clear();
        }

        public void AddIpBlock(string address)
        {
            IPBlockedList.AddIfNotContains(new RouteIPBlock(Id, address));
        }

        public void RemoveIpBlock(string address)
        {
            IPBlockedList.RemoveAll(ip => ip.Address.Equals(address));
        }

        public void RemoveAllIpBlock()
        {
            IPBlockedList.Clear();
        }
    }
}
