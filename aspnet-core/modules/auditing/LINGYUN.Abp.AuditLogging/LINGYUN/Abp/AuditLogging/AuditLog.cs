using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging
{
    [DisableAuditing]
    public class AuditLog : IHasExtraProperties
    {
        public Guid Id { get; set; }

        public string ApplicationName { get; set; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public Guid? TenantId { get; set; }

        public string TenantName { get; set; }

        public Guid? ImpersonatorUserId { get; set; }

        public Guid? ImpersonatorTenantId { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string ClientId { get; set; }

        public string CorrelationId { get; set; }

        public string BrowserInfo { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }

        public string Exceptions { get; set; }

        public string Comments { get; set; }

        public int? HttpStatusCode { get; set; }

        public List<EntityChange> EntityChanges { get; set; }

        public List<AuditLogAction> Actions { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public AuditLog()
        {
            Actions = new List<AuditLogAction>();
            EntityChanges = new List<EntityChange>();
            ExtraProperties = new ExtraPropertyDictionary();
        }

        public AuditLog(
            Guid id,
            string applicationName,
            Guid? tenantId,
            string tenantName,
            Guid? userId,
            string userName,
            DateTime executionTime,
            int executionDuration,
            string clientIpAddress,
            string clientName,
            string clientId,
            string correlationId,
            string browserInfo,
            string httpMethod,
            string url,
            int? httpStatusCode,
            Guid? impersonatorUserId,
            Guid? impersonatorTenantId,
            ExtraPropertyDictionary extraPropertyDictionary,
            List<EntityChange> entityChanges,
            List<AuditLogAction> actions,
            string exceptions,
            string comments)
        {
            Id = id;
            ApplicationName = applicationName;
            TenantId = tenantId;
            TenantName = tenantName;
            UserId = userId;
            UserName = userName;
            ExecutionTime = executionTime;
            ExecutionDuration = executionDuration;
            ClientIpAddress = clientIpAddress;
            ClientName = clientName;
            ClientId = clientId;
            CorrelationId = correlationId;
            BrowserInfo = browserInfo;
            HttpMethod = httpMethod;
            Url = url;
            HttpStatusCode = httpStatusCode;
            ImpersonatorUserId = impersonatorUserId;
            ImpersonatorTenantId = impersonatorTenantId;

            ExtraProperties = extraPropertyDictionary;
            EntityChanges = entityChanges;
            Actions = actions;
            Exceptions = exceptions;
            Comments = comments;
        }
    }
}
