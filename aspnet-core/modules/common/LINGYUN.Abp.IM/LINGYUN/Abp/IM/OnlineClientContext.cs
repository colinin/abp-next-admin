using System;

namespace LINGYUN.Abp.IM
{
    public class OnlineClientContext
    {
        public string ConnectionId { get; }

        public Guid? TenantId { get; }

        public Guid UserId { get; }

        public OnlineClientContext(string connectionId, Guid userId, Guid? tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
            ConnectionId = connectionId;
        }
    }
}
