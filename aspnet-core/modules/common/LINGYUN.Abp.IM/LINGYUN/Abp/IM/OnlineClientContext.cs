using System;

namespace LINGYUN.Abp.IM
{
    public class OnlineClientContext
    {
        public Guid? TenantId { get; }

        public Guid UserId { get; }

        public OnlineClientContext(Guid userId, Guid? tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }
    }
}
