using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleInGroup : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid RuleId { get; protected set; }
        public virtual Guid GroupId { get; protected set; }
        protected EntityRuleInGroup()
        {
        }

        public EntityRuleInGroup(
            [NotNull] Guid ruleId,
            [NotNull] Guid groupId,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(ruleId, nameof(ruleId));
            Check.NotNull(groupId, nameof(groupId));

            RuleId = ruleId;
            GroupId = groupId;
            TenantId = tenantId;
        }
        public override object[] GetKeys()
        {
            return new object[] { RuleId, GroupId };
        }
    }
}
