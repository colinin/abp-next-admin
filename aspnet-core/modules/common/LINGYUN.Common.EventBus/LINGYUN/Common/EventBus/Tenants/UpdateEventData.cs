using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.Update)]
    public class UpdateEventData
    {
        public Guid Id { get; set; }

        public string OriginName { get; set; }

        public string Name { get; set; }
    }
}
