using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.DeleteConnectionString)]
    public class DeleteConnectionStringEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
