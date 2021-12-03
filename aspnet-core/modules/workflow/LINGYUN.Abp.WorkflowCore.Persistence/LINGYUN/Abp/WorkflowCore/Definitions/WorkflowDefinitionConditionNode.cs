using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinitionConditionNode : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid ParentId { get; protected set; }
        public virtual WorkflowDefinitionNode Node { get; protected set; }
        public virtual string Label { get; protected set; }
        public virtual string NodeId { get; protected set; }
        public virtual ICollection<WorkflowDefinitionConditionCondition> Conditions { get; protected set; }

        protected WorkflowDefinitionConditionNode()
        {
            Conditions = new Collection<WorkflowDefinitionConditionCondition>();
        }

        public WorkflowDefinitionConditionNode(
            Guid parentId,
            string label,
            string nodeId,
            Guid? tenantId = null)
        {
            ParentId = parentId;
            Label = label;
            NodeId = nodeId;
            TenantId = tenantId;

            Conditions = new Collection<WorkflowDefinitionConditionCondition>();
        }
    }
}
