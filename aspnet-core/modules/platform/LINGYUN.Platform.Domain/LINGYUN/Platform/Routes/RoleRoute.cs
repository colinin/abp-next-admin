using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes
{
    public class RoleRoute : FullAuditedEntity<int>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string RoleName { get; protected set; }
        public virtual Guid RouteId { get; protected set; }
        protected RoleRoute()
        {

        }

        public RoleRoute(int id)
        {
            Id = id;
        }

        public RoleRoute(Guid routeId, [NotNull] string roleName, Guid? tenantId = null)
        {
            RouteId = routeId;
            TenantId = tenantId;
            RoleName = Check.NotNullOrWhiteSpace(roleName, nameof(roleName));
        }
    }
}
