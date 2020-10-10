using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntitySubRule : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid RuleId { get; protected set; }
        public virtual Guid SubId { get; protected set; }
        protected EntitySubRule()
        {
        }

        public EntitySubRule(
            [NotNull] Guid ruleId,
            [NotNull] Guid subId,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(ruleId, nameof(ruleId));
            Check.NotNull(subId, nameof(subId));

            RuleId = ruleId;
            SubId = subId;
            TenantId = tenantId;
        }
        public override object[] GetKeys()
        {
            return new object[] { RuleId, SubId };
        }
    }
}
