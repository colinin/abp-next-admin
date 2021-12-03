using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinitionNode : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid WorkflowId { get; protected set; }
        public virtual WorkflowDefinition Workflow { get; protected set; }
        public virtual string Key { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Position { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual WorkflowDefinitionStepBody StepBody { get; protected set; }
        public virtual string ParentNodes { get; protected set; }
        public virtual ICollection<WorkflowDefinitionConditionNode> NextNodes { get; protected set; }
        protected WorkflowDefinitionNode() 
        {
            NextNodes = new Collection<WorkflowDefinitionConditionNode>();
        }
        public WorkflowDefinitionNode(
            Guid workflowId,
            string key,
            string title,
            int[] position,
            string type,
            WorkflowDefinitionStepBody stepBody,
            string[] parentNodes,
            Guid? tenantId = null)
        {
            WorkflowId = workflowId;
            Key = key;
            Title = title;
            Position = position.JoinAsString(";");
            Type = type;
            StepBody = stepBody;
            ParentNodes = parentNodes.JoinAsString(";");
            TenantId = tenantId;

            NextNodes = new Collection<WorkflowDefinitionConditionNode>();
        }
    }
}
