using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class WorkflowExecutionError : Entity<int>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid WorkflowId { get; protected set; }

        public virtual Guid ExecutionPointerId { get; set; }

        public virtual DateTime ErrorTime { get; set; }

        public virtual string Message { get; set; }

        protected WorkflowExecutionError() { }
        public WorkflowExecutionError(
            Guid workflowId,
            Guid executionPointerId,
            DateTime errorTime,
            string message,
            Guid? tenantId = null)
        {
            WorkflowId = workflowId;
            ExecutionPointerId = executionPointerId;
            ErrorTime = errorTime;
            Message = message;
            TenantId = tenantId;
        }
    }
}
