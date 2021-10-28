using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging
{
    [DisableAuditing]
    public class AuditLogAction : IHasExtraProperties
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public Guid AuditLogId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public AuditLogAction()
        {
            ExtraProperties = new ExtraPropertyDictionary();
        }

        public AuditLogAction(Guid id, Guid auditLogId, AuditLogActionInfo actionInfo, Guid? tenantId = null)
        {

            Id = id;
            TenantId = tenantId;
            AuditLogId = auditLogId;
            ExecutionTime = actionInfo.ExecutionTime;
            ExecutionDuration = actionInfo.ExecutionDuration;
            ExtraProperties = new ExtraPropertyDictionary(actionInfo.ExtraProperties);
            ServiceName = actionInfo.ServiceName;
            MethodName = actionInfo.MethodName;
            Parameters = actionInfo.Parameters.Length > 2000 ? "" : actionInfo.Parameters;
        }
    }
}
