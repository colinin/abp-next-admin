using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleInject : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 规则标识
        /// </summary>
        public virtual Guid RuleId { get; protected set; }
        /// <summary>
        /// 依赖的规则标识
        /// </summary>
        public virtual Guid InjectId { get; protected set; }

        protected EntityRuleInject() { }
        public EntityRuleInject(
            [NotNull] Guid ruleId,
            [NotNull] Guid injectId,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(ruleId, nameof(ruleId));
            Check.NotNull(injectId, nameof(injectId));

            RuleId = ruleId;
            InjectId = injectId;
            TenantId = tenantId;
        }
        public override object[] GetKeys()
        {
            return new object[] { RuleId , InjectId };
        }
    }
}
