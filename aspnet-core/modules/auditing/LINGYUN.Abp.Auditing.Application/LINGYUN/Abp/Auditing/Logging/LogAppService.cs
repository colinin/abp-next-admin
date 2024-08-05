using LINGYUN.Abp.Auditing.Features;
using LINGYUN.Abp.Auditing.Permissions;
using LINGYUN.Abp.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Auditing.Logging;

[Authorize(AuditingPermissionNames.SystemLog.Default)]
[RequiresFeature(AuditingFeatureNames.Logging.SystemLog)]
public class LogAppService : AuditingApplicationServiceBase, ILogAppService
{
    private readonly ILoggingManager _manager;

    public LogAppService(ILoggingManager manager)
    {
        _manager = manager;
    }

    public async virtual Task<LogDto> GetAsync(string id)
    {
        var log = await _manager.GetAsync(id);

        return ObjectMapper.Map<LogInfo, LogDto>(log);
    }

    public async virtual Task<PagedResultDto<LogDto>> GetListAsync(LogGetByPagedDto input)
    {
        var count = await _manager.GetCountAsync(
            input.StartTime, input.EndTime, input.Level,
            input.MachineName, input.Environment,
            input.Application, input.Context, 
            input.RequestId,  input.RequestPath,
            input.CorrelationId, input.ProcessId, 
            input.ThreadId, input.HasException);

        var logs = await _manager.GetListAsync(
            input.Sorting, input.MaxResultCount, input.SkipCount,
            input.StartTime, input.EndTime, input.Level,
            input.MachineName, input.Environment,
            input.Application, input.Context,
            input.RequestId, input.RequestPath,
            input.CorrelationId, input.ProcessId,
            input.ThreadId, input.HasException,
            includeDetails: false);

        return new PagedResultDto<LogDto>(count,
            ObjectMapper.Map<List<LogInfo>, List<LogDto>>(logs));
    }
}
