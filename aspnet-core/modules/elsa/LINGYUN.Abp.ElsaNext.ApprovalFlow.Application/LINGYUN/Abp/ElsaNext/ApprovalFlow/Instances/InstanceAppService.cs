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

	protected IApprovalInstanceStarter InstanceStarter => LazyServiceProvider.LazyGetRequiredService<IApprovalInstanceStarter>();

	protected IApprovalInstanceTerminationHandler TerminationHandler => LazyServiceProvider.LazyGetRequiredService<IApprovalInstanceTerminationHandler>();

	protected IApprovalDecisionHandler DecisionHandler => LazyServiceProvider.LazyGetRequiredService<IApprovalDecisionHandler>();

	protected IApprovalNodeTransferHandler TransferHandler => LazyServiceProvider.LazyGetRequiredService<IApprovalNodeTransferHandler>();

	protected IApprovalNodeAdditionHandler AdditionHandler => LazyServiceProvider.LazyGetRequiredService<IApprovalNodeAdditionHandler>();

	protected IApprovalActionResolver ActionResolver => LazyServiceProvider.LazyGetRequiredService<IApprovalActionResolver>();

	protected IApprovalNodeTaskRepository NodeTaskRepository => LazyServiceProvider.LazyGetRequiredService<IApprovalNodeTaskRepository>();

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Create)]
	public virtual async Task<InstanceDto> StartAsync(InstanceStartDto input)
	{
		UserTaskActor actor = await GetRequiredActorAsync();
		return MapInstance(await InstanceStarter.StartAsync(new ApprovalInstanceStartRequest(input.TemplateId, input.Title, actor.Subject, input.BusinessKey, input.FormDataJson)));
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
	public virtual async Task<InstanceDto> GetAsync(Guid id)
	{
		return MapInstance(await InstanceRepository.GetAsync(id));
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
			Instance = MapInstance(instance),
			Records = records,
			NodeTasks = nodeTasks.Select(nodeTask => new NodeTaskDto
			{
				NodeId = nodeTask.NodeId,
				NodeName = nodeTask.NodeName,
				TaskId = nodeTask.TaskId,
				Status = nodeTask.Status.ToString(),
				ApproverDisplayName = nodeTask.ApproverDisplayName,
				SignMode = nodeTask.SignMode.ToString()
			}).ToList(),
			FlowDefinitionJson = template?.FlowDefinitionJson,
			FormDefinitionId = template?.FormDefinitionId
		};
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
	public virtual async Task<PagedResultDto<InstanceDto>> GetPagedListAsync(InstanceGetPagedListInput input)
	{
		var actor = await GetRequiredActorAsync();
		Expression<Func<ApprovalInstance, bool>> filter = _ => true;
		if (input.OnlyMine)
		{
			filter = ExpressionFuncExtender.And(filter, x => x.InitiatorProvider == actor.Subject.Provider && x.InitiatorType == actor.Subject.Type.ToString() && x.InitiatorId == actor.Subject.Id);
		}
		if (input.TemplateId.HasValue)
		{
			filter = ExpressionFuncExtender.And(filter, x => x.TemplateId == input.TemplateId.Value);
		}
		if (!input.Status.IsNullOrWhiteSpace() && Enum.TryParse<ApprovalInstanceStatus>(input.Status, ignoreCase: true, out var status))
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
            (await InstanceRepository.GetListAsync(specification, input.Sorting, input.MaxResultCount, input.SkipCount)).Select(MapInstance).ToList());
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Decide)]
	public virtual async Task<InstanceDecisionResultDto> DecideAsync(Guid id, InstanceDecisionDto input)
	{
		var actor = await GetRequiredActorAsync();
		var effect = await ResolveEffectAsync(id, input.ActionKey, actor);
		var operationId = (input.OperationId.IsNullOrWhiteSpace() ? Guid.NewGuid().ToString("N") : input.OperationId);
		var result = await DecisionHandler.DecideAsync(new ApprovalDecisionRequest(id, input.TaskId, input.ActionKey, effect, actor, operationId, input.Comment, input.DataJson));
		return new InstanceDecisionResultDto
		{
			RecordId = result.Record?.Id,
			Ignored = result.Ignored
		};
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
	public virtual async Task<InstanceDto> ReturnAsync(Guid id, InstanceReturnDto input)
	{
		var actor = await GetRequiredActorAsync();
		await TerminationHandler.ReturnAsync(id, actor, input.Comment);
		return MapInstance(await InstanceRepository.GetAsync(id));
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
	public virtual async Task<InstanceDto> TransferAsync(Guid id, InstanceTransferDto input)
	{
		var actor = await GetRequiredActorAsync();
		var instance = await InstanceRepository.GetAsync(id);
		await TransferHandler.TransferAsync(new ApprovalNodeTransferRequest(id, ToParticipant(instance, input.Transferee), actor, input.TaskId, input.Comment));
		return MapInstance(instance);
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
	public virtual async Task<InstanceDto> AddApprovalAsync(Guid id, InstanceAddApprovalDto input)
	{
		var actor = await GetRequiredActorAsync();
		var instance = await InstanceRepository.GetAsync(id);
		await AdditionHandler.AddNodeAsync(new ApprovalNodeAdditionRequest(id, ToParticipant(instance, input.Approver), actor, input.TaskId, input.Title, null, input.Comment));
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
		return MapInstance(instance);
	}

	[Authorize(ElsaNextApprovalFlowPermissions.Instances.Withdraw)]
	public virtual async Task<InstanceDto> WithdrawAsync(Guid id, InstanceWithdrawDto input)
	{
		var actor = await GetRequiredActorAsync();
		await TerminationHandler.WithdrawAsync(id, actor, input.Comment);
		return MapInstance(await InstanceRepository.GetAsync(id));
	}

	protected virtual async Task<ApprovalActionEffect> ResolveEffectAsync(Guid instanceId, string actionKey, UserTaskActor actor)
	{
		var action = (await ActionResolver.ResolveAsync(instanceId, actor)).FirstOrDefault(x => string.Equals(x.Key, actionKey, StringComparison.OrdinalIgnoreCase));
		if (action != null)
		{
			return action.Effect;
		}
		if (actionKey.Contains(RejectActionKey, StringComparison.OrdinalIgnoreCase) || actionKey.Contains(RefuseActionKey, StringComparison.OrdinalIgnoreCase))
		{
			return ApprovalActionEffect.Reject;
		}
		return ApprovalActionEffect.Approve;
	}
}
