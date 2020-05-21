using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Common.EventBus.Tenants
{
    [EventName(TenantEventNames.CreateConnectionString)]
    public class CreateConnectionStringEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
