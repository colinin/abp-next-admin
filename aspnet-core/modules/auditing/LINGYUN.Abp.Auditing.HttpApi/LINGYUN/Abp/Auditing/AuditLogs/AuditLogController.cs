using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Auditing.AuditLogs
{
    [RemoteService(Name = AuditingRemoteServiceConsts.RemoteServiceName)]
    [Area("auditing")]
    [ControllerName("audit-log")]
    [Route("api/auditing/audit-log")]
    public class AuditLogController : AbpController, IAuditLogAppService
    {
        protected IAuditLogAppService AuditLogAppService { get; }

        public AuditLogController(IAuditLogAppService auditLogAppService)
        {
            AuditLogAppService = auditLogAppService;
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await AuditLogAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<AuditLogDto> GetAsync(Guid id)
        {
            return await AuditLogAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input)
        {
            return await AuditLogAppService.GetListAsync(input);
        }
    }
}
