using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class RouteQos : Entity<int>
    {
        public virtual Guid QoSId { get; protected set; }
        public virtual QoS QoS { get; protected set; }
        public virtual Guid RouteId { get; protected set; }
        public virtual Route Route { get; protected set; }
        protected RouteQos()
        {

        }
        public RouteQos(Guid routeId, Guid qosId)
        {
            RouteId = routeId;
            QoSId = qosId;
        }
    }
}
