using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Auditing.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input);

        Task<AuditLogDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);
    }
}
