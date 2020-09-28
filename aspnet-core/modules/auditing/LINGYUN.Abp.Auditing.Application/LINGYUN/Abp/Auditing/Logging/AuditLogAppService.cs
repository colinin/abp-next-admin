using LINGYUN.Abp.Auditing.Features;
using LINGYUN.Abp.Auditing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Auditing.Logging
{
    [Authorize(AuditingPermissionNames.AuditLog.Default)]
    [RequiresFeature(AuditingFeatureNames.Logging.AuditLog)]
    public class AuditLogAppService : AuditingApplicationServiceBase, IAuditLogAppService
    {
        protected IAuditLogRepository AuditLogRepository { get; }

        public AuditLogAppService(IAuditLogRepository auditLogRepository)
        {
            AuditLogRepository = auditLogRepository;
        }

        public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input)
        {
            var auditLogCount = await AuditLogRepository
                .GetCountAsync(input.StartTime, input.EndTime,
                    input.HttpMethod, input.Url, input.UserName, input.ApplicationName,
                    input.CorrelationId, input.MaxExecutionDuration, input.MinExecutionDuration,
                    input.HasException, input.HttpStatusCode);

            var auditLog = await AuditLogRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                input.StartTime, input.EndTime,
                    input.HttpMethod, input.Url, input.UserName, input.ApplicationName,
                    input.CorrelationId, input.MaxExecutionDuration, input.MinExecutionDuration,
                    input.HasException, input.HttpStatusCode, true);

            return new PagedResultDto<AuditLogDto>(auditLogCount,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(auditLog));
        }

        [Authorize(AuditingPermissionNames.AuditLog.Delete)]
        public virtual async Task DeleteAsync([Required] Guid id)
        {
            var auditLog = await AuditLogRepository.GetAsync(id, false);
            await AuditLogRepository.DeleteAsync(auditLog);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
