using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleGroup : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string DisplayName { get; protected set; }
        public virtual string EntityFullTypeName { get; protected set; }
        public virtual ICollection<EntityRuleInGroup> Rules { get; protected set; }
        public virtual ICollection<EntityRuleInject> InjectRules { get; protected set; }
        protected EntityRuleGroup()
        {
        }

        public EntityRuleGroup(
            [NotNull] Guid id,
            [NotNull] string name,
            [NotNull] string entiyFullTypeName,
            [CanBeNull] string displayName,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNullOrWhiteSpace(name, nameof(name), EntityRuleGroupConsts.MaxNameLength);
            Check.NotNullOrWhiteSpace(entiyFullTypeName, nameof(entiyFullTypeName), EntityRuleGroupConsts.MaxEntiyFullTypeNameLength);

            Id = id;
            Name = name;
            DisplayName = displayName;
            TenantId = tenantId;

            Rules = new Collection<EntityRuleInGroup>();
            InjectRules = new Collection<EntityRuleInject>();
            ExtraProperties = new Dictionary<string, object>();
        }

        public virtual void AddInjectRule([NotNull] EntityRule rule)
        {
            Check.NotNull(rule, nameof(rule));
            if (IsInjectRule(rule.Id))
            {
                return;
            }
            InjectRules.Add(new EntityRuleInject(rule.Id, Id, TenantId));
        }

        public virtual void RemoveInjectRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));
            if (IsInjectRule(ruleId))
            {
                return;
            }
            InjectRules.RemoveAll(rule => rule.RuleId == ruleId);
        }

        public virtual bool IsInjectRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));

            return InjectRules.Any(rule => rule.RuleId == ruleId);
        }


        public virtual void AddRule([NotNull] EntityRule rule)
        {
            Check.NotNull(rule, nameof(rule));

            if (IsInRule(rule.Id))
            {
                return;
            }
            Rules.Add(new EntityRuleInGroup(rule.Id, Id, TenantId));
        }

        public virtual void RemoveRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));

            if (!IsInRule(ruleId))
            {
                return;
            }

            Rules.RemoveAll(r => r.RuleId == ruleId);
        }

        public virtual bool IsInRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));

            return Rules.Any(r => r.RuleId == ruleId);
        }
    }
}
