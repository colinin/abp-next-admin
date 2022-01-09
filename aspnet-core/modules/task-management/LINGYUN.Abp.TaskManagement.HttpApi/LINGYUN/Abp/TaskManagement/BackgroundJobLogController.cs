using LINGYUN.Abp.TaskManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

[RemoteService(Name = TaskManagementRemoteServiceConsts.RemoteServiceName)]
[Area(TaskManagementRemoteServiceConsts.ModuleName)]
[Authorize(TaskManagementPermissions.BackgroundJobLogs.Default)]
[Route($"api/{TaskManagementRemoteServiceConsts.ModuleName}/background-jobs/logs")]
public class BackgroundJobLogController : TaskManagementController, IBackgroundJobLogAppService
{
    protected IBackgroundJobLogAppService BackgroundJobLogAppService { get; }

    public BackgroundJobLogController(
        IBackgroundJobLogAppService backgroundJobLogAppService)
    {
        BackgroundJobLogAppService = backgroundJobLogAppService;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(TaskManagementPermissions.BackgroundJobLogs.Delete)]
    public Task DeleteAsync(long id)
    {
        return BackgroundJobLogAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<BackgroundJobLogDto> GetAsync(long id)
    {
        return BackgroundJobLogAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<BackgroundJobLogDto>> GetListAsync(BackgroundJobLogGetListInput input)
    {
        return BackgroundJobLogAppService.GetListAsync(input);
    }
}
