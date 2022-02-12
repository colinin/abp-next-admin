using System;

namespace LINGYUN.Abp.MultiTenancy
{
    public class ConnectionStringDeletedEventData
    {
        public Guid TenantId { get; set; }

        public string TenantName { get; set; }

        public string Name { get; set; }
    }
}
