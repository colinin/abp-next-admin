using Elsa.ApprovalFlow;
using Elsa.ApprovalFlow.Activities;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks;

[Authorize]
public partial class TaskAppService : ApprovalFlowAppServiceBase, ITaskAppService, IApplicationService, IRemoteService
{
    private const string ApprovalTaskType = ApprovalTask.TaskTypeMarker;

    protected IUserTaskManager UserTaskManager { get; }

    protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

    protected IApprovalInstanceRepository InstanceRepository { get; }

    protected IApprovalRecordRepository RecordRepository { get; }

    public TaskAppService(
        IUserTaskManager userTaskManager,
        IApprovalNodeTaskRepository nodeTaskRepository,
        IApprovalInstanceRepository instanceRepository,
        IApprovalRecordRepository recordRepository)
    {
        UserTaskManager = userTaskManager;
        NodeTaskRepository = nodeTaskRepository;
        InstanceRepository = instanceRepository;
        RecordRepository = recordRepository;
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<ListResultDto<TaskDto>> GetPendingTasksAsync()
    {
        var actor = await GetRequiredActorAsync();
        var summaries = new Dictionary<string, UserTaskSummary>(StringComparer.Ordinal);

        // Approval tasks are either assigned to the current user or available as candidates.
        // Load both scopes and return the complete set; the frontend grid performs paging.
        foreach (var scope in new[] { UserTaskQueryScopeKind.Assigned, UserTaskQueryScopeKind.Available })
        {
            string? cursor = null;
            while (true)
            {
                var tasks = await UserTaskManager.QueryAsync(
                    new UserTaskQuery
                    {
                        TenantId = actor.Subject.TenantId,
                        TaskType = ApprovalTaskType,
                        Statuses =
                        [
                            UserTaskStatus.Unassigned,
                            UserTaskStatus.Available,
                            UserTaskStatus.Assigned,
                        ],
                        Cursor = cursor,
                        Limit = 200,
                        IncludeTotalCount = false,
                    },
                    scope,
                    actor);

                if (tasks == null)
                {
                    break;
                }

                foreach (var task in tasks.Items)
                {
                    summaries[task.Id] = task;
                }

                cursor = tasks.NextCursor;
                if (string.IsNullOrWhiteSpace(cursor))
                {
                    break;
                }
            }
        }

        var result = new List<TaskDto>();
        foreach (var task in summaries.Values)
        {
            var nodeTask = await NodeTaskRepository.FindByTaskIdAsync(task.Id);
            if (nodeTask == null)
            {
                continue;
            }

            var instance = await InstanceRepository.FindAsync(nodeTask.InstanceId);
            if (instance?.IsFinished() ?? true)
            {
                continue;
            }

            result.Add(new TaskDto
            {
                TaskId = task.Id,
                InstanceId = instance.Id,
                InstanceTitle = instance.Title,
                TemplateName = instance.TemplateName,
                NodeId = nodeTask.NodeId,
                NodeName = nodeTask.NodeName,
                Title = task.Title,
                Summary = task.Summary,
                Priority = task.Priority,
                DueAt = task.DueAt,
                CreatedAt = task.CreatedAt,
                Revision = task.Revision,
                AllowedActions = ApprovalTaskTags.ReadActionEffects(task.Tags).Keys.ToArray(),
            });
        }

        return new ListResultDto<TaskDto>(
            result.OrderByDescending(x => x.CreatedAt).ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task MarkViewedAsync(string taskId)
    {
        var actor = await GetRequiredActorAsync();
        var nodeTask = await NodeTaskRepository.FindByTaskIdAsync(taskId);
        if (nodeTask == null)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
        }

        var tenantId = ApprovalInstanceGuard.ToUserTaskTenantId(
            await InstanceRepository.GetAsync(nodeTask.InstanceId));
        if (await UserTaskManager.GetAsync(tenantId, taskId, actor) == null)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.NoPendingTask);
        }

        nodeTask.MarkFirstViewed(new DateTimeOffset(Clock.Now));
        await NodeTaskRepository.UpdateAsync(nodeTask, autoSave: true);
    }
}
