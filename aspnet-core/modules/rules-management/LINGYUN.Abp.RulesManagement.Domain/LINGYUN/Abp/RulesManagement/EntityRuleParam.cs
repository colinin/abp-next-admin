using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleParam : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid RuleId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Expression { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        protected EntityRuleParam() 
        {
        }
        public EntityRuleParam(
            [NotNull] Guid id,
            [NotNull] Guid ruleId,
            [NotNull] string name,
            [NotNull] string expression,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(ruleId, nameof(ruleId));
            Check.NotNullOrWhiteSpace(name, nameof(name), EntityRuleParamConsts.MaxNameLength);
            Check.NotNullOrWhiteSpace(expression, nameof(expression), EntityRuleParamConsts.MaxExpressionLength);

            Id = id;
            RuleId = ruleId;
            Name = name;
            Expression = expression;
            TenantId = tenantId;

            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
