using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Auditing.Logging
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input);

        Task DeleteAsync(Guid id);
    }
}
