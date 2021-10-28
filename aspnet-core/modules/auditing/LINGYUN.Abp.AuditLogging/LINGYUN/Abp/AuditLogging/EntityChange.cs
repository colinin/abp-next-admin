using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.AuditLogging
{
    [DisableAuditing]
    public class EntityChange : IHasExtraProperties
    {
        public Guid Id { get; set; }

        public Guid AuditLogId { get; set; }

        public Guid? TenantId { get; set; }

        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public Guid? EntityTenantId { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public List<EntityPropertyChange> PropertyChanges { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public EntityChange()
        {
            PropertyChanges = new List<EntityPropertyChange>();
            ExtraProperties = new ExtraPropertyDictionary();
        }

        public EntityChange(
            IGuidGenerator guidGenerator,
            Guid auditLogId,
            EntityChangeInfo entityChangeInfo,
            Guid? tenantId = null)
        {
            Id = guidGenerator.Create();
            AuditLogId = auditLogId;
            TenantId = tenantId;
            ChangeTime = entityChangeInfo.ChangeTime;
            ChangeType = entityChangeInfo.ChangeType;
            EntityId = entityChangeInfo.EntityId;
            EntityTypeFullName = entityChangeInfo.EntityTypeFullName;

            PropertyChanges = entityChangeInfo
                                  .PropertyChanges?
                                  .Select(p => new EntityPropertyChange(guidGenerator, Id, p, tenantId))
                                  .ToList()
                              ?? new List<EntityPropertyChange>();

            ExtraProperties = new ExtraPropertyDictionary();
            if (entityChangeInfo.ExtraProperties != null)
            {
                foreach (var pair in entityChangeInfo.ExtraProperties)
                {
                    ExtraProperties.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
