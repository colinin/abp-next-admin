using JetBrains.Annotations;
using LINGYUN.Abp.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRule : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; protected set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual string Operator { get; protected set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public virtual string ErrorMessage { get; protected set; }
        /// <summary>
        /// 错误类型
        /// </summary>
        public virtual ErrorType ErrorType { get; protected set; }
        /// <summary>
        /// 表达式类型
        /// </summary>
        public virtual ExpressionType? ExpressionType { get; set; }
        /// <summary>
        /// 所属规则
        /// </summary>
        public virtual Guid? ParentId { get; protected set; }

        public virtual ICollection<EntitySubRule> SubRules { get; protected set; }
        public virtual ICollection<EntityRuleInject> InjectRules { get; protected set; }
        public virtual ICollection<EntityRuleParam> Params { get; protected set; }
        public virtual string Expression { get; protected set; }

        protected EntityRule()
        {
        }

        public EntityRule(
            [NotNull] Guid id,
            [NotNull] string name,
            [CanBeNull] string operation,
            [CanBeNull] string displayName,
            [CanBeNull] Guid? parentId = null,
            [CanBeNull] Guid? tenantId = null)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNullOrWhiteSpace(name, nameof(name), EntityRuleConsts.MaxNameLength);

            Id = id;
            Name = name;
            Operator = operation;
            DisplayName = displayName;
            ParentId = parentId;
            TenantId = tenantId;

            ExpressionType = Abp.Rules.ExpressionType.LambdaExpression;

            Params = new Collection<EntityRuleParam>();
            SubRules = new Collection<EntitySubRule>();
            InjectRules = new Collection<EntityRuleInject>();
            ExtraProperties = new Dictionary<string, object>();
        }

        public virtual void SetErrorInfomation(string errorMessage, ErrorType errorType = ErrorType.Warning)
        {
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }

        public virtual void AddParamter([NotNull] IGuidGenerator guidGenerator, [NotNull] string name, [NotNull] string expression)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));

            if (Params.Any(p => p.Name == name))
            {
                return;
            }
            Params.Add(new EntityRuleParam(guidGenerator.Create(), Id, name, expression, TenantId));
        }

        public virtual void RemoveParamter([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Params.RemoveAll(p => p.Name == name);
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


        public virtual void AddSubRule([NotNull] EntityRule rule)
        {
            Check.NotNull(rule, nameof(rule));

            if (IsInRule(rule.Id))
            {
                return;
            }
            SubRules.Add(new EntitySubRule(Id, rule.Id, TenantId));
        }

        public virtual void RemoveSubRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));

            if (!IsInRule(ruleId))
            {
                return;
            }

            SubRules.RemoveAll(r => r.SubId == ruleId);
        }

        public virtual bool IsInRule([NotNull] Guid ruleId)
        {
            Check.NotNull(ruleId, nameof(ruleId));

            return SubRules.Any(r => r.SubId == ruleId);
        }
    }
}
