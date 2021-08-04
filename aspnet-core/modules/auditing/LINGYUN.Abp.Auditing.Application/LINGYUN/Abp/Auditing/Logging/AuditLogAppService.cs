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

        public virtual async Task<AuditLogDto> GetAsync(Guid id)
        {
            var auditLog = await AuditLogRepository.GetAsync(id, includeDetails: true);

            return ObjectMapper.Map<AuditLog, AuditLogDto>(auditLog);
        }

        public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input)
        {
            var auditLogCount = await AuditLogRepository
                .GetCountAsync(input.StartTime, input.EndTime,
                    input.HttpMethod, input.Url, 
                    input.UserId, input.UserName, 
                    input.ApplicationName, input.CorrelationId, 
                    input.MaxExecutionDuration, input.MinExecutionDuration,
                    input.HasException, input.HttpStatusCode);

            var auditLogs = await AuditLogRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                input.StartTime, input.EndTime,
                    input.HttpMethod, input.Url, 
                    input.UserId, input.UserName, 
                    input.ApplicationName, input.CorrelationId, 
                    input.MaxExecutionDuration, input.MinExecutionDuration,
                    input.HasException, input.HttpStatusCode, includeDetails: false);

            return new PagedResultDto<AuditLogDto>(auditLogCount,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(auditLogs));
        }

        [Authorize(AuditingPermissionNames.AuditLog.Delete)]
        public virtual async Task DeleteAsync([Required] Guid id)
        {
            var auditLog = await AuditLogRepository.GetAsync(id);
            await AuditLogRepository.DeleteAsync(auditLog);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
