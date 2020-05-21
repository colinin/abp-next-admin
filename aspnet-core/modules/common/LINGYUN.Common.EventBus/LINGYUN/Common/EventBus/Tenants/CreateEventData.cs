using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.Create)]
    public class CreateEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AdminEmailAddress { get; set; }

        public string AdminPassword { get; set; }
    }
}
