using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Auditing.SecurityLogs
{
    public class SecurityLogDto : ExtensibleEntityDto<Guid>
    {
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
    }
}
