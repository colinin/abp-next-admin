using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.Delete)]
    public class DeleteEventData
    {
        public Guid Id { get; set; }
    }
}
