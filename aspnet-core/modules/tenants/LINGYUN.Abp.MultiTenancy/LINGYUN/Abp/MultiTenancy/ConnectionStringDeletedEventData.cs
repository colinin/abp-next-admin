using System;

namespace LINGYUN.Abp.MultiTenancy
{
    public class ConnectionStringDeletedEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
