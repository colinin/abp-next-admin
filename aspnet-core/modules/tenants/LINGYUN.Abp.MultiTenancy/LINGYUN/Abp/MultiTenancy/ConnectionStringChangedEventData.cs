using System;

namespace LINGYUN.Abp.MultiTenancy
{
    public class ConnectionStringChangedEventData
    {
        public Guid Id { get; set; }

        public string OriginName { get; set; }

        public string Name { get; set; }
    }
}
