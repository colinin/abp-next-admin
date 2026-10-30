using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/tasks")]
public class TaskController : AbpControllerBase, ITaskAppService
{
    protected ITaskAppService TaskAppService { get; }

    public TaskController(ITaskAppService taskAppService)
    {
        TaskAppService = taskAppService;
    }

    [HttpGet]
    public virtual Task<ListResultDto<TaskDto>> GetPendingTasksAsync()
    {
        return TaskAppService.GetPendingTasksAsync();
    }

    [HttpPost("{taskId}/view")]
    public virtual Task MarkViewedAsync(string taskId)
    {
        return TaskAppService.MarkViewedAsync(taskId);
    }

    [HttpGet("done")]
    public virtual Task<PagedResultDto<DoneDto>> GetDoneTasksAsync(
        [FromQuery] DoneGetPagedListInput input)
    {
        return TaskAppService.GetDoneTasksAsync(input);
    }
}
