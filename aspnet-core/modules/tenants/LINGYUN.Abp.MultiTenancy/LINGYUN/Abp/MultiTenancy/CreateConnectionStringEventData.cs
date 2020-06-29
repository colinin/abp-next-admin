using System;

namespace LINGYUN.Abp.MultiTenancy
{
    public class CreateConnectionStringEventData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
