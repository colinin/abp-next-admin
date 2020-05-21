using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.UpdateConnectionString)]
    public class UpdateConnectionStringEventData
    {
        public Guid Id { get; set; }

        public string OriginName { get; set; }

        public string Name { get; set; }

        public string OriginValue { get; set; }

        public string Value { get; set; }
    }
}
