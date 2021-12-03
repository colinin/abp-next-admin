using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinitionFormData : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid WorkflowId { get; protected set; }
        public virtual WorkflowDefinition Workflow { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Label { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual string Value { get; protected set; }
        public virtual ExtraPropertyDictionary Styles { get; protected set; }
        public virtual int? MaxLength { get; protected set; }
        public virtual int? MinLength { get; protected set; }
        public virtual ExtraPropertyDictionary Items { get; protected set; }
        public virtual ExtraPropertyDictionary Rules { get; protected set; }
        protected WorkflowDefinitionFormData()
        {
            Styles = new ExtraPropertyDictionary();
            Items = new ExtraPropertyDictionary();
            Rules = new ExtraPropertyDictionary();
        }
        public WorkflowDefinitionFormData(
            Guid workflowId,
            string name,
            string label,
            string type,
            string value,
            int? minLength = null,
            int? maxLength = null,
            Guid? tenantId = null)
        {
            WorkflowId = workflowId;
            Name = name;
            Label = label;
            Type = type;
            Value = value;
            MinLength = minLength;
            MaxLength = maxLength;
            TenantId = tenantId;

            Styles = new ExtraPropertyDictionary();
            Items = new ExtraPropertyDictionary();
            Rules = new ExtraPropertyDictionary();
        }
    }
}
