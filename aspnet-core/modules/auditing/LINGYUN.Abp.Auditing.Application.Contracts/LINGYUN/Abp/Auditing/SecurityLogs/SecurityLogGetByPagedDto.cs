using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Auditing.SecurityLogs
{
    public class SecurityLogGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ApplicationName { get; set; }
        public string Identity { get; set; }
        public string ActionName { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string CorrelationId { get; set; }
    }
}
