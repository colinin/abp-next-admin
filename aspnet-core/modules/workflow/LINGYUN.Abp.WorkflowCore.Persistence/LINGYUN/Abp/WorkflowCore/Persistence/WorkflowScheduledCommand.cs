using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class WorkflowScheduledCommand : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string CommandName { get; set; }

        public virtual string Data { get; set; }

        public virtual long ExecuteTime { get; set; }
        protected WorkflowScheduledCommand() { }
        public WorkflowScheduledCommand(
            string commandName,
            string data,
            long executeTime,
            Guid? tenantId = null)
        {
            CommandName = commandName;
            Data = data;
            ExecuteTime = executeTime;
            TenantId = tenantId;
        }
    }
}
