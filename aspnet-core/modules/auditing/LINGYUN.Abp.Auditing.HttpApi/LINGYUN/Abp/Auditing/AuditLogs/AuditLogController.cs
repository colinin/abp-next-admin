using Asp.Versioning;
using LINGYUN.Abp.Auditing.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Auditing.AuditLogs;

[RemoteService(Name = AuditingRemoteServiceConsts.RemoteServiceName)]
[Area("auditing")]
[ControllerName("audit-log")]
[Route("api/auditing/audit-log")]
[Authorize(AuditingPermissionNames.AuditLog.Default)]
public class AuditLogController : AbpControllerBase, IAuditLogAppService
{
    protected IAuditLogAppService AuditLogAppService { get; }

    public AuditLogController(IAuditLogAppService auditLogAppService)
    {
        AuditLogAppService = auditLogAppService;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AuditingPermissionNames.AuditLog.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        await AuditLogAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public async virtual Task<AuditLogDto> GetAsync(Guid id)
    {
        return await AuditLogAppService.GetAsync(id);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetByPagedDto input)
    {
        return await AuditLogAppService.GetListAsync(input);
    }
}
