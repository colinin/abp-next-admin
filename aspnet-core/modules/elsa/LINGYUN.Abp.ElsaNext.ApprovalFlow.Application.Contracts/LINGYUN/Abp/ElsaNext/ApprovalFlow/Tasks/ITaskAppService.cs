using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks;

public interface ITaskAppService : IApplicationService
{
    /// <summary>
    /// 获取当前用户的待办任务列表
    /// </summary>
    Task<ListResultDto<TaskDto>> GetPendingTasksAsync();

    /// <summary>
    /// 获取当前用户的已办任务列表
    /// </summary>
    Task<PagedResultDto<DoneDto>> GetDoneTasksAsync(DoneGetPagedListInput input);

    /// <summary>
    /// 记录审批人首次查看待办的时间。
    /// </summary>
    Task MarkViewedAsync(string taskId);
}
