using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.UserTasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public class ApprovalInstanceStarter : DomainService, IApprovalInstanceStarter
{
	public const string InstanceIdInputKey = ElsaNextApprovalFlowConsts.InstanceIdInputKey;

	public const string InitiatorInputKey = ElsaNextApprovalFlowConsts.InitiatorInputKey;

	public const string BusinessKeyInputKey = ElsaNextApprovalFlowConsts.BusinessKeyInputKey;

	public const string FormDataInputKey = ElsaNextApprovalFlowConsts.FormDataInputKey;

	public const string ApprovalFormReferenceKey = ElsaNextApprovalFlowConsts.ApprovalFormReferenceKey;

	public const string ApprovalTitleInputKey = ElsaNextApprovalFlowConsts.ApprovalTitleInputKey;

	protected IApprovalTemplateRepository TemplateRepository { get; }

	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	protected IApprovalWorkflowStarter WorkflowStarter { get; }

	public ApprovalInstanceStarter(IApprovalTemplateRepository templateRepository, IApprovalInstanceRepository instanceRepository, IApprovalRecordRepository recordRepository, IApprovalWorkflowStarter workflowStarter)
	{
		TemplateRepository = templateRepository;
		InstanceRepository = instanceRepository;
		RecordRepository = recordRepository;
		WorkflowStarter = workflowStarter;
	}

	public virtual async Task<ApprovalInstance> StartAsync(ApprovalInstanceStartRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Check.NotNull(request, nameof(request));
		ApprovalTemplate template = await TemplateRepository.GetAsync(request.TemplateId, includeDetails: true, cancellationToken);
		if (!template.CanStart())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.NotPublished);
		}
		ApprovalInstance instance = new ApprovalInstance(GuidGenerator.Create(), template.Id, template.DisplayName, request.Title, request.Initiator, template.Category, request.BusinessKey, request.FormDataJson, new DateTimeOffset(Clock.Now));
		string workflowInstanceId = GuidGenerator.Create().ToString("N");
		await WorkflowStarter.StartAsync(new ApprovalWorkflowStartRequest(template.ElsaDefinitionId, workflowInstanceId, instance.Id.ToString("D"), BuildInput(instance, template, request)), cancellationToken);
		instance.SetElsaWorkflowInstanceId(workflowInstanceId);
		await InstanceRepository.InsertAsync(instance, autoSave: true, cancellationToken);
		await RecordRepository.InsertAsync(new ApprovalRecord(GuidGenerator.Create(), instance.Id, ApprovalRecordAction.Start, ApprovalActionEffect.Neutral, new DateTimeOffset(Clock.Now), request.Initiator), autoSave: true, cancellationToken);
		return instance;
	}

	protected virtual IReadOnlyDictionary<string, object?> BuildInput(ApprovalInstance instance, ApprovalTemplate template, ApprovalInstanceStartRequest request)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			[InstanceIdInputKey] = instance.Id.ToString("D"),
			[BusinessKeyInputKey] = request.BusinessKey,
			[ApprovalTitleInputKey] = instance.Title,
			[FormDataInputKey] = request.FormDataJson
		};
		if (request.Initiator != null)
		{
			dictionary[InitiatorInputKey] = request.Initiator;
		}
		if (template.FormDefinitionId.HasValue)
		{
			dictionary[ApprovalFormReferenceKey] = new UserTaskFormReference(FormDefinitionUserTaskFormProvider.ProviderName, template.FormDefinitionId.Value.ToString("D"));
		}
		return dictionary;
	}
}
