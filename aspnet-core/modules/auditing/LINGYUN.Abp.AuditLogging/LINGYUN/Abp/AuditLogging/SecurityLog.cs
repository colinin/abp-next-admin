using System;
using Volo.Abp.Data;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging
{
    public class SecurityLog : IHasExtraProperties
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string ApplicationName { get; set; }

        public string Identity { get; set; }

        public string Action { get; set; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public string TenantName { get; set; }

        public string ClientId { get; set; }

        public string CorrelationId { get; set; }

        public string ClientIpAddress { get; set; }

        public string BrowserInfo { get; set; }

        public DateTime CreationTime { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public SecurityLog()
        {
            ExtraProperties = new ExtraPropertyDictionary();
        }

        public SecurityLog(Guid id, SecurityLogInfo securityLogInfo)
        {
            Id = id;
            TenantId = securityLogInfo.TenantId;
            TenantName = securityLogInfo.TenantName;

            ApplicationName = securityLogInfo.ApplicationName;
            Identity = securityLogInfo.Identity;
            Action = securityLogInfo.Action;

            UserId = securityLogInfo.UserId;
            UserName = securityLogInfo.UserName;

            CreationTime = securityLogInfo.CreationTime;

            ClientIpAddress = securityLogInfo.ClientIpAddress;
            ClientId = securityLogInfo.ClientId;
            CorrelationId = securityLogInfo.CorrelationId;
            BrowserInfo = securityLogInfo.BrowserInfo;

            ExtraProperties = new ExtraPropertyDictionary();
            if (securityLogInfo.ExtraProperties != null)
            {
                foreach (var pair in securityLogInfo.ExtraProperties)
                {
                    ExtraProperties.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
