using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes
{
    public class UserRoute : FullAuditedEntity<int>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual Guid RouteId { get; protected set; }
        protected UserRoute()
        {

        }

        public UserRoute(int id)
        {
            Id = id;
        }

        public UserRoute(Guid routeId, Guid userId, Guid? tenantId = null)
        {
            UserId = userId;
            RouteId = routeId;
            TenantId = tenantId;
        }
    }
}
