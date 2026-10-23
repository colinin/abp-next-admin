using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

public abstract class ApprovalFlowAppServiceBase : ApplicationService
{
    protected IApprovalActorAccessor ActorAccessor => LazyServiceProvider.LazyGetRequiredService<IApprovalActorAccessor>();
    protected IApprovalTemplateRepository TemplateRepository => LazyServiceProvider.LazyGetRequiredService<IApprovalTemplateRepository>();
    protected IApprovalInstanceRepository InstanceRepository => LazyServiceProvider.LazyGetRequiredService<IApprovalInstanceRepository>();
    protected IApprovalRecordRepository RecordRepository => LazyServiceProvider.LazyGetRequiredService<IApprovalRecordRepository>();
    protected IApprovalNodeTaskRepository NodeTaskRepository => LazyServiceProvider.LazyGetRequiredService<IApprovalNodeTaskRepository>();

    protected virtual async Task<UserTaskActor> GetRequiredActorAsync()
    {
        var actor = await ActorAccessor.GetAsync();
        if (actor == null)
        {
            throw new AbpAuthorizationException("The current user could not be resolved as an approval actor.");
        }

        return actor;
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

    protected virtual TemplateDto MapTemplate(ApprovalTemplate template)
    {
        return new TemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            DisplayName = template.DisplayName,
            Description = template.Description,
            Category = template.Category,
            Icon = template.Icon,
            BusinessKeyHint = template.BusinessKeyHint,
            IsPublished = template.IsPublished,
            IsEnabled = template.IsEnabled,
            ElsaDefinitionId = template.ElsaDefinitionId,
            ElsaDefinitionKey = template.ElsaDefinitionKey,
            ElsaDefinitionVersion = template.ElsaDefinitionVersion,
            FormDefinitionId = template.FormDefinitionId,
            FlowDefinitionJson = template.FlowDefinitionJson,
        };
    }

    protected virtual InstanceDto MapInstance(ApprovalInstance instance)
    {
        return new InstanceDto
        {
            Id = instance.Id,
            TemplateId = instance.TemplateId,
            TemplateName = instance.TemplateName,
            TemplateCategory = instance.TemplateCategory,
            BusinessKey = instance.BusinessKey,
            Title = instance.Title,
            Status = instance.Status.ToString(),
            CurrentNodeId = instance.CurrentNodeId,
            CurrentNodeName = instance.CurrentNodeName,
            InitiatorId = instance.InitiatorId,
            InitiatorDisplayName = instance.InitiatorDisplayName,
            ElsaWorkflowInstanceId = instance.ElsaWorkflowInstanceId,
            FormDataJson = instance.FormDataJson,
            Outcome = instance.Outcome,
            StartedAt = instance.StartedAt,
            FinishedAt = instance.FinishedAt,
        };
    }

    protected virtual RecordDto MapRecord(ApprovalRecord record, ApprovalNodeTask? nodeTask = null)
    {
        return new RecordDto
        {
            Id = record.Id,
            InstanceId = record.InstanceId,
            NodeId = record.NodeId,
            NodeName = record.NodeName,
            TaskId = record.TaskId,
            ActionKey = record.ActionKey,
            ActionLabel = record.ActionLabel,
            Effect = record.Effect.ToString(),
            Comment = record.Comment,
            ActorDisplayName = record.ActorDisplayName ?? "系统",
            OccurredAt = record.OccurredAt,
            FirstViewedAt = nodeTask?.FirstViewedAt,
            CompletedAt = record.OccurredAt,
        };
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

        var result = new List<RecordDto>();
        foreach (var record in records)
        {
            var nodeTask = record.TaskId == null
                ? null
                : await NodeTaskRepository.FindByTaskIdAsync(record.TaskId);
            result.Add(MapRecord(record, nodeTask));
        }

        return result;
    }
}
