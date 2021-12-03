using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public string Title { get; protected set; }
        public int Version { get; protected set; }
        public string Description { get; protected set; }
        public string Icon { get; protected set; }
        public string Color { get; protected set; }
        public string Group { get; protected set; }
        public ICollection<WorkflowDefinitionNode> Nodes { get; protected set; }
        public ICollection<WorkflowDefinitionFormData> Inputs { get; protected set; }
        protected WorkflowDefinition()
        {
            Nodes = new Collection<WorkflowDefinitionNode>();
            Inputs = new Collection<WorkflowDefinitionFormData>();
        }
        public WorkflowDefinition(
            Guid id,
            string title,
            int version,
            string group,
            string icon,
            string color,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            Title = title;
            Version = version;
            Group = group;
            Icon = icon;
            Color = color;
            Description = description;
            TenantId = tenantId;

            Nodes = new Collection<WorkflowDefinitionNode>();
            Inputs = new Collection<WorkflowDefinitionFormData>();
        }
    }
}
