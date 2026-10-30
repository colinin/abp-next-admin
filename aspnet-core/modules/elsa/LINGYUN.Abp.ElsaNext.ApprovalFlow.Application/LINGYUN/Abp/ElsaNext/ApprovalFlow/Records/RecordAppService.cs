using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

[Authorize]
public class RecordAppService : ApprovalFlowAppServiceBase, IRecordAppService
{
    protected IApprovalRecordRepository RecordRepository { get; }

    protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

    public RecordAppService(
        IApprovalRecordRepository recordRepository,
        IApprovalNodeTaskRepository nodeTaskRepository)
    {
        RecordRepository = recordRepository;
        NodeTaskRepository = nodeTaskRepository;
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<ListResultDto<RecordDto>> GetListAsync(Guid instanceId)
    {
        var records = await GetRecordDtosAsync(instanceId);
        return new ListResultDto<RecordDto>(records);
    }

    protected virtual async Task<List<RecordDto>> GetRecordDtosAsync(Guid instanceId)
    {
        Expression<Func<ApprovalRecord, bool>> filter = x => x.InstanceId == instanceId;
        var specification = new ExpressionSpecification<ApprovalRecord>(filter);

        var totalCount = await RecordRepository.GetCountAsync(specification);
        var records = await RecordRepository.GetListAsync(
            specification,
            sorting: nameof(ApprovalRecord.OccurredAt),
            maxResultCount: totalCount);

        var result = new List<RecordDto>(records.Count);
        foreach (var record in records)
        {
            var nodeTask = record.TaskId == null
                ? null
                : await NodeTaskRepository.FindByTaskIdAsync(record.TaskId);

            var dto = ObjectMapper.Map<ApprovalRecord, RecordDto>(record);
            dto.ActorDisplayName ??= "系统";
            dto.FirstViewedAt = nodeTask?.FirstViewedAt;
            dto.CompletedAt = record.OccurredAt;
            result.Add(dto);
        }

        return result;
    }
}
