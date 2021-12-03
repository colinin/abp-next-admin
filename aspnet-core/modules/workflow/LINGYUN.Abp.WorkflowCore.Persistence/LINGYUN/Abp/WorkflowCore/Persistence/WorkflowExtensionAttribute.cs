using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class WorkflowExtensionAttribute : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid ExecutionPointerId { get; set; }

        public virtual WorkflowExecutionPointer ExecutionPointer { get; set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        protected WorkflowExtensionAttribute() { }

        public WorkflowExtensionAttribute(
            Guid pointerId,
            string key,
            string value)
        {
            ExecutionPointerId = pointerId;
            Key = key;
            Value = value;
        }
    }
}
