using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

[Authorize]
public class InstanceAppService : ApprovalFlowAppServiceBase, IInstanceAppService, IApplicationService, IRemoteService
{
    private const string RejectActionKey = "reject";

    private const string RefuseActionKey = "refuse";

    protected IApprovalInstanceRepository InstanceRepository { get; }

    protected IApprovalTemplateRepository TemplateRepository { get; }

    protected IApprovalRecordRepository RecordRepository { get; }

    protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

    protected IApprovalInstanceStarter InstanceStarter { get; }

    protected IApprovalInstanceTerminationHandler TerminationHandler { get; }

    protected IApprovalDecisionHandler DecisionHandler { get; }

    protected IApprovalNodeTransferHandler TransferHandler { get; }

    protected IApprovalNodeAdditionHandler AdditionHandler { get; }

    protected IApprovalActionResolver ActionResolver { get; }

    public InstanceAppService(
        IApprovalInstanceRepository instanceRepository,
        IApprovalTemplateRepository templateRepository,
        IApprovalRecordRepository recordRepository,
        IApprovalNodeTaskRepository nodeTaskRepository,
        IApprovalInstanceStarter instanceStarter,
        IApprovalInstanceTerminationHandler terminationHandler,
        IApprovalDecisionHandler decisionHandler,
        IApprovalNodeTransferHandler transferHandler,
        IApprovalNodeAdditionHandler additionHandler,
        IApprovalActionResolver actionResolver)
    {
        InstanceRepository = instanceRepository;
        TemplateRepository = templateRepository;
        RecordRepository = recordRepository;
        NodeTaskRepository = nodeTaskRepository;
        InstanceStarter = instanceStarter;
        TerminationHandler = terminationHandler;
        DecisionHandler = decisionHandler;
        TransferHandler = transferHandler;
        AdditionHandler = additionHandler;
        ActionResolver = actionResolver;
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Create)]
    public virtual async Task<InstanceDto> StartAsync(InstanceStartDto input)
    {
        var actor = await GetRequiredActorAsync();
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(await InstanceStarter.StartAsync(
            new ApprovalInstanceStartRequest(
                input.TemplateId,
                input.Title,
                actor.Subject,
                input.BusinessKey,
                input.FormDataJson)));
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<InstanceDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(await InstanceRepository.GetAsync(id));
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<InstanceDetailDto> GetDetailAsync(Guid id)
    {
        var instance = await InstanceRepository.GetAsync(id);
        var records = await GetRecordDtosAsync(id);
        var nodeTasks = await NodeTaskRepository.GetListByInstanceAsync(id);
        var template = await TemplateRepository.FindAsync(instance.TemplateId);
        return new InstanceDetailDto
        {
            Instance = ObjectMapper.Map<ApprovalInstance, InstanceDto>(instance),
            Records = records,
            NodeTasks = nodeTasks.Select(nodeTask => ObjectMapper.Map<ApprovalNodeTask, NodeTaskDto>(nodeTask)).ToList(),
            FlowDefinitionJson = template?.FlowDefinitionJson,
            FormDefinitionId = template?.FormDefinitionId,
        };
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<PagedResultDto<InstanceDto>> GetPagedListAsync(InstanceGetPagedListInput input)
    {
        var actor = await GetRequiredActorAsync();
        Expression<Func<ApprovalInstance, bool>> filter = _ => true;
        if (input.OnlyMine)
        {
            filter = ExpressionFuncExtender.And(
                filter,
                x => x.InitiatorProvider == actor.Subject.Provider &&
                    x.InitiatorType == actor.Subject.Type.ToString() &&
                    x.InitiatorId == actor.Subject.Id);
        }
        if (input.TemplateId.HasValue)
        {
            filter = ExpressionFuncExtender.And(filter, x => x.TemplateId == input.TemplateId.Value);
        }
        if (!input.Status.IsNullOrWhiteSpace() &&
            Enum.TryParse<ApprovalInstanceStatus>(input.Status, ignoreCase: true, out var status))
        {
            filter = ExpressionFuncExtender.And(filter, x => (int)x.Status == (int)status);
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            filter = ExpressionFuncExtender.And(filter, x => x.Title.Contains(input.Filter));
        }

        var specification = new ExpressionSpecification<ApprovalInstance>(filter);
        return new PagedResultDto<InstanceDto>(
            await InstanceRepository.GetCountAsync(specification),
            (await InstanceRepository.GetListAsync(
                specification,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount))
            .Select(instance => ObjectMapper.Map<ApprovalInstance, InstanceDto>(instance))
            .ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Decide)]
    public virtual async Task<InstanceDecisionResultDto> DecideAsync(Guid id, InstanceDecisionDto input)
    {
        var actor = await GetRequiredActorAsync();
        var effect = await ResolveEffectAsync(id, input.ActionKey, actor);
        var operationId = input.OperationId.IsNullOrWhiteSpace()
            ? Guid.NewGuid().ToString("N")
            : input.OperationId;
        var result = await DecisionHandler.DecideAsync(
            new ApprovalDecisionRequest(
                id,
                input.TaskId,
                input.ActionKey,
                effect,
                actor,
                operationId,
                input.Comment,
                input.DataJson));
        return new InstanceDecisionResultDto
        {
            RecordId = result.Record?.Id,
            Ignored = result.Ignored,
        };
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<InstanceDto> ReturnAsync(Guid id, InstanceReturnDto input)
    {
        var actor = await GetRequiredActorAsync();
        await TerminationHandler.ReturnAsync(id, actor, input.Comment);
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(await InstanceRepository.GetAsync(id));
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<InstanceDto> TransferAsync(Guid id, InstanceTransferDto input)
    {
        var actor = await GetRequiredActorAsync();
        var instance = await InstanceRepository.GetAsync(id);
        await TransferHandler.TransferAsync(
            new ApprovalNodeTransferRequest(
                id,
                ToParticipant(instance, input.Transferee),
                actor,
                input.TaskId,
                input.Comment));
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(instance);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<InstanceDto> AddApprovalAsync(Guid id, InstanceAddApprovalDto input)
    {
        var actor = await GetRequiredActorAsync();
        var instance = await InstanceRepository.GetAsync(id);
        await AdditionHandler.AddNodeAsync(
            new ApprovalNodeAdditionRequest(
                id,
                ToParticipant(instance, input.Approver),
                actor,
                input.TaskId,
                input.Title,
                null,
                input.Comment));
        if (!string.IsNullOrWhiteSpace(input.TaskId))
        {
            await DecisionHandler.DecideAsync(
                new ApprovalDecisionRequest(
                    id,
                    input.TaskId,
                    ApprovalRecordAction.AddApproval.ToString(),
                    ApprovalActionEffect.Neutral,
                    actor,
                    Guid.NewGuid().ToString("N"),
                    input.Comment));
        }
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(instance);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Withdraw)]
    public virtual async Task<InstanceDto> WithdrawAsync(Guid id, InstanceWithdrawDto input)
    {
        var actor = await GetRequiredActorAsync();
        await TerminationHandler.WithdrawAsync(id, actor, input.Comment);
        return ObjectMapper.Map<ApprovalInstance, InstanceDto>(await InstanceRepository.GetAsync(id));
    }

    protected virtual async Task<ApprovalActionEffect> ResolveEffectAsync(
        Guid instanceId,
        string actionKey,
        UserTaskActor actor)
    {
        var action = (await ActionResolver.ResolveAsync(instanceId, actor))
            .FirstOrDefault(x => string.Equals(x.Key, actionKey, StringComparison.OrdinalIgnoreCase));
        if (action != null)
        {
            return action.Effect;
        }
        if (actionKey.Contains(RejectActionKey, StringComparison.OrdinalIgnoreCase) ||
            actionKey.Contains(RefuseActionKey, StringComparison.OrdinalIgnoreCase))
        {
            return ApprovalActionEffect.Reject;
        }
        return ApprovalActionEffect.Approve;
    }

    protected virtual ParticipantReference ToParticipant(
        ApprovalInstance instance,
        InstanceParticipantDto input)
    {
        var tenantId = ApprovalInstanceGuard.ToUserTaskTenantId(instance);
        var type = Enum.TryParse<UserTaskParticipantType>(input.Type, true, out var parsed)
            ? parsed
            : UserTaskParticipantType.User;
        return new ParticipantReference(
            tenantId,
            string.IsNullOrWhiteSpace(input.Provider) ? ParticipantConsts.DefaultProvider : input.Provider,
            type,
            input.Id,
            input.DisplayName);
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
