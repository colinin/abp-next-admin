using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Definitions
{
    public class WorkflowDefinitionStepBody : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public ExtraPropertyDictionary ExtraProperties { get; protected set; }
        protected WorkflowDefinitionStepBody()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
        public WorkflowDefinitionStepBody(
            Guid id,
            string name) : base(id)
        {
            Name = name;

            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }
}
