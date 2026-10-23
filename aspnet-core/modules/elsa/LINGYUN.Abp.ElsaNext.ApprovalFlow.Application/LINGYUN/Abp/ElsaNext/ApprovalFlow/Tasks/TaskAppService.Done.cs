using Elsa.ApprovalFlow.Models;
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
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks;

[Authorize]
public partial class TaskAppService
{
    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<PagedResultDto<DoneDto>> GetDoneTasksAsync(DoneGetPagedListInput input)
    {
        var actor = await GetRequiredActorAsync();
        var actorType = actor.Subject.Type.ToString();

        Expression<Func<ApprovalRecord, bool>> filter = x =>
            x.ActorId == actor.Subject.Id &&
            x.ActorType == actorType &&
            x.Effect != ApprovalActionEffect.Neutral;

        if (!input.Filter.IsNullOrWhiteSpace())
        {
            filter = ExpressionFuncExtender.And(
                filter,
                x => x.NodeName != null && x.NodeName.Contains(input.Filter!));
        }

        var specification = new ExpressionSpecification<ApprovalRecord>(filter);
        var totalCount = await RecordRepository.GetCountAsync(specification);
        var records = await RecordRepository.GetListAsync(
            specification,
            sorting: $"{nameof(ApprovalRecord.OccurredAt)} DESC",
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount);

        var result = new List<DoneDto>();
        foreach (var record in records)
        {
            var instance = await InstanceRepository.FindAsync(record.InstanceId);
            if (instance == null)
            {
                continue;
            }

            var nodeTask = record.TaskId == null
                ? null
                : await NodeTaskRepository.FindByTaskIdAsync(record.TaskId);

            result.Add(new DoneDto
            {
                Id = record.Id,
                InstanceId = record.InstanceId,
                InstanceTitle = instance.Title,
                TemplateName = instance.TemplateName,
                NodeId = record.NodeId,
                NodeName = record.NodeName,
                ActionKey = record.ActionKey,
                ActionLabel = record.ActionLabel,
                Effect = record.Effect.ToString(),
                Comment = record.Comment,
                ActorDisplayName = record.ActorDisplayName ?? "系统",
                DecidedAt = record.OccurredAt,
                FirstViewedAt = nodeTask?.FirstViewedAt,
                CompletedAt = record.OccurredAt,
            });
        }

        return new PagedResultDto<DoneDto>(totalCount, result);
    }
}