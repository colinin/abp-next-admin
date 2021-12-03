using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinitionConditionCondition : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid ParentId { get; protected set; }
        public virtual WorkflowDefinitionConditionNode ConditionNode { get; protected set; }
        public virtual string Field { get; set; }
        public virtual string Operator { get; set; }
        public virtual string Value { get; set; }
        protected WorkflowDefinitionConditionCondition() { }
        public WorkflowDefinitionConditionCondition(
            Guid parentId,
            string field,
            string opt,
            string value,
            Guid? tenantId = null)
        {
            ParentId = parentId;
            Field = field;
            Operator = opt;
            Value = value;
            TenantId = tenantId;
        }
    }
}
