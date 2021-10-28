using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Auditing.AuditLogs
{
    public class EntityPropertyChangeDto : EntityDto<Guid>
    {
        public string NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeFullName { get; set; }
    }
}
