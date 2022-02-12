using LINGYUN.Abp.Auditing.Features;
using LINGYUN.Abp.Auditing.Permissions;
using LINGYUN.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Auditing.SecurityLogs
{
    [Authorize(AuditingPermissionNames.SecurityLog.Default)]
    [RequiresFeature(AuditingFeatureNames.Logging.SecurityLog)]
    public class SecurityLogAppService : AuditingApplicationServiceBase, ISecurityLogAppService
    {
        protected ISecurityLogManager SecurityLogManager { get; }
        public SecurityLogAppService(ISecurityLogManager securityLogManager)
        {
            SecurityLogManager = securityLogManager;
        }

        public virtual async Task<SecurityLogDto> GetAsync(Guid id)
        {
            var securityLog = await SecurityLogManager.GetAsync(id, includeDetails: true);

            return ObjectMapper.Map<SecurityLog, SecurityLogDto>(securityLog);
        }

        public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetByPagedDto input)
        {
            var securityLogCount = await SecurityLogManager
                .GetCountAsync(input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId
                );

            var securityLogs = await SecurityLogManager
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                    input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId,
                    includeDetails: false
                );

            return new PagedResultDto<SecurityLogDto>(securityLogCount,
                ObjectMapper.Map<List<SecurityLog>, List<SecurityLogDto>>(securityLogs));
        }

        [Authorize(AuditingPermissionNames.SecurityLog.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await SecurityLogManager.DeleteAsync(id);
        }
    }
}
