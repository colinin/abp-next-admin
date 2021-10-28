using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.Auditing.AuditLogs
{
    public class EntityChangeDto : ExtensibleEntityDto<Guid>
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public Guid? EntityTenantId { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }
        public List<EntityPropertyChangeDto> PropertyChanges { get; set; }
        public EntityChangeDto()
        {
            PropertyChanges = new List<EntityPropertyChangeDto>();
        }
    }
}
