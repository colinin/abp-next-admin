using LINGYUN.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account;

[Authorize]
public class MySecurityLogAppService : AccountApplicationServiceBase, IMySecurityLogAppService
{
    protected ISecurityLogManager SecurityLogManager { get; }
    public MySecurityLogAppService(ISecurityLogManager securityLogManager)
    {
        SecurityLogManager = securityLogManager;
    }
    public async virtual Task<SecurityLogDto> GetAsync(Guid id)
    {
        var securityLog = await SecurityLogManager.GetAsync(id, includeDetails: true);

        return ObjectMapper.Map<SecurityLog, SecurityLogDto>(securityLog);
    }

    public async virtual Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input)
    {
        var userId = CurrentUser.GetId();
        var securityLogCount = await SecurityLogManager
            .GetCountAsync(input.StartTime, input.EndTime,
                input.ApplicationName, input.Identity, input.ActionName,
                userId, null, input.ClientId, input.CorrelationId
            );

        var securityLogs = await SecurityLogManager
            .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                input.StartTime, input.EndTime,
                input.ApplicationName, input.Identity, input.ActionName,
                userId, null, input.ClientId, input.CorrelationId,
                includeDetails: false
            );

        return new PagedResultDto<SecurityLogDto>(securityLogCount,
            ObjectMapper.Map<List<SecurityLog>, List<SecurityLogDto>>(securityLogs));
    }

    public async virtual Task DeleteAsync(Guid id)
    {
        await SecurityLogManager.DeleteAsync(id);
    }
}
