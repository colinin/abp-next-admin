using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleGroupEto : IMultiTenant, ICreationAuditedObject, IModificationAuditedObject
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
