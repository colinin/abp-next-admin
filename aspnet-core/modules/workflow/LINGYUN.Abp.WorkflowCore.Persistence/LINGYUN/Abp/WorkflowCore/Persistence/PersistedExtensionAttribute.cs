using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class PersistedExtensionAttribute : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid ExecutionPointerId { get; protected set; }

        public virtual PersistedExecutionPointer ExecutionPointer { get; protected set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        protected PersistedExtensionAttribute() { }

        public PersistedExtensionAttribute(
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
