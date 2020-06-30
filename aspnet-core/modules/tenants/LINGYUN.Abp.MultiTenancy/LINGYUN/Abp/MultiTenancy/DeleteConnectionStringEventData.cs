using System;

namespace LINGYUN.Abp.MultiTenancy
{
    public class DeleteConnectionStringEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
