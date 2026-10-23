using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using Elsa.ApprovalFlow.Activities;
using Elsa.ApprovalFlow.Models;
using Elsa.Expressions.JavaScript.Models;
using Elsa.UserTasks.Models;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Management;
using Elsa.Workflows.Management.Entities;
using Elsa.Workflows.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

/// <summary>
/// Compiles the designer JSON (<see cref="ApprovalTemplate.FlowDefinitionJson"/>) into an Elsa workflow definition.
/// Publishing a template no longer requires an externally created Elsa definition id: the definition is generated
/// and published here, and its version id is stored on the template for dispatching.
/// </summary>
public class ApprovalWorkflowCompiler : DomainService, IApprovalWorkflowCompiler
{
    protected IWorkflowDefinitionPublisher DefinitionPublisher { get; }

    protected IWorkflowTemplateCompiler WorkflowTemplateCompiler { get; }

    public ApprovalWorkflowCompiler(
        IWorkflowDefinitionPublisher definitionPublisher,
        IWorkflowTemplateCompiler workflowTemplateCompiler)
    {
        DefinitionPublisher = definitionPublisher;
        WorkflowTemplateCompiler = workflowTemplateCompiler;
    }

    public virtual async Task<WorkflowDefinition> CompileAsync(ApprovalTemplate template, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(template.FlowDefinitionJson))
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.DesignRequired)
                ;
        }

        Sequence sequence;
        try
        {
            using var jsonDocument = JsonDocument.Parse(template.FlowDefinitionJson);
            var rootElement = jsonDocument.RootElement;

            if (WorkflowTemplateCompiler.CanCompile(rootElement))
            {
                // AntFlow designs use their own model and are compiled by the AntFlowDesign domain service.
                sequence = WorkflowTemplateCompiler.Compile(rootElement, template);
            }
            else
            {
                // The legacy { nodes: [{ id, kind }] } document is compiled here.
                var design = rootElement.Deserialize<ApprovalDesignDocument>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var tenantId = template.TenantId?.ToString("D") ?? "";
                sequence = CompileNodes(design?.Nodes ?? [], template, tenantId);
            }
        }
        catch (JsonException exception)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.WorkflowInvalid, innerException: exception);
        }

        if (sequence.Activities.Count == 0)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.WorkflowInvalid);
        }

        var definition = await DefinitionPublisher.NewAsync(sequence, cancellationToken);
        definition.Name = template.DisplayName;
        definition.Description = template.Description;

        // The compiled condition expressions read the submitted form data through this workflow input.
        definition.Inputs.Add(new InputDefinition
        {
            Name = ElsaNextApprovalFlowConsts.FormDataInputKey,
            Type = typeof(string),
        });

        var result = await DefinitionPublisher.PublishAsync(definition, cancellationToken);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.ValidationErrors.Select(error => error.Message));
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.WorkflowInvalid);
        }

        Logger.LogInformation(
            "Approval template {TemplateId} published as Elsa workflow definition {DefinitionId} (version {Version}).",
            template.Id,
            definition.DefinitionId,
            definition.Version);

        return definition;
    }

    /// <summary>
    /// Compiles the linear node list of one flow level into a sequence. Condition nodes become <see cref="If"/>
    /// activities whose branches are compiled recursively.
    /// </summary>
    protected virtual Sequence CompileNodes(
        IReadOnlyCollection<ApprovalDesignNode> nodes,
        ApprovalTemplate template,
        string tenantId)
    {
        var sequence = new Sequence();

        foreach (var node in nodes)
        {
            if (string.Equals(node.Kind, "approval", StringComparison.OrdinalIgnoreCase))
            {
                sequence.Activities.Add(BuildApprovalTask(node, template, tenantId));
            }
            else if (string.Equals(node.Kind, "cc", StringComparison.OrdinalIgnoreCase))
            {
                sequence.Activities.Add(BuildApprovalCc(node, template, tenantId));
            }
            else if (string.Equals(node.Kind, "condition", StringComparison.OrdinalIgnoreCase))
            {
                sequence.Activities.Add(BuildCondition(node, template, tenantId));
            }
        }

        return sequence;
    }

    protected virtual ApprovalTask BuildApprovalTask(
        ApprovalDesignNode node,
        ApprovalTemplate template,
        string tenantId)
    {
        var rules = node.Rules.Select(rule => MapRule(rule, tenantId)).ToArray();

        return new ApprovalTask
        {
            Title = new Input<string>(node.Name),
            Summary = new Input<string?>(template.Description),
            ApprovalNodeKey = new Input<string?>(node.Id),
            ApprovalNodeName = new Input<string?>(node.Name),
            ApproverRules = new Input<IReadOnlyCollection<ApproverRule>?>(rules),
            SignMode = new Input<ApprovalSignMode>(ParseSignMode(node.SignMode)),
            Actions = new Input<IReadOnlyCollection<ApprovalActionDefinition>?>(MapActions(node.Actions)),
            OnEmpty = new Input<EmptyApproverPolicy>(ParseEmptyPolicy(node.OnEmpty)),
        };
    }

    protected virtual ApprovalCc BuildApprovalCc(
        ApprovalDesignNode node,
        ApprovalTemplate template,
        string tenantId)
    {
        var rules = node.Rules.Select(rule => MapRule(rule, tenantId)).ToArray();

        return new ApprovalCc
        {
            Title = new Input<string>(node.Name),
            Summary = new Input<string?>(template.Description),
            RecipientRules = new Input<IReadOnlyCollection<ApproverRule>?>(rules),
        };
    }

    protected virtual Activity BuildCondition(
        ApprovalDesignNode node,
        ApprovalTemplate template,
        string tenantId)
    {
        var branches = node.Branches ?? [];
        if (branches.Count == 0)
        {
            return new Sequence();
        }

        Activity BuildChain(int index)
        {
            // 最后一个分支是默认条件，不参与条件判断，直接执行其节点。
            if (index >= branches.Count - 1)
            {
                return CompileNodes(branches[^1].Nodes, template, tenantId);
            }

            var branch = branches[index];
            var expression = branch.Expression;
            if (string.IsNullOrWhiteSpace(expression) && index == 0)
            {
                expression = node.Condition?.Expression;
            }

            if (string.IsNullOrWhiteSpace(expression))
            {
                return BuildChain(index + 1);
            }

            return new If
            {
                Condition = new Input<bool>(JavaScriptExpression.Create(ToJavaScript(expression))),
                Then = CompileNodes(branch.Nodes, template, tenantId),
                Else = BuildChain(index + 1),
            };
        }

        return BuildChain(0);
    }

    protected virtual ApproverRule MapRule(ApprovalDesignRule rule, string tenantId)
    {
        var kind = Enum.TryParse<ApproverKind>(rule.Kind, true, out var parsedKind)
            ? parsedKind
            : ApproverKind.Participant;

        return new ApproverRule
        {
            Kind = kind,
            Participants = rule.Participants.Select(participant => new ParticipantReference(
                tenantId,
                string.IsNullOrWhiteSpace(participant.Provider) ? ParticipantConsts.DefaultProvider : participant.Provider,
                Enum.TryParse<UserTaskParticipantType>(participant.Type, true, out var participantType)
                    ? participantType
                    : UserTaskParticipantType.User,
                participant.Id,
                participant.DisplayName)).ToArray(),
            Level = rule.Level ?? 1,
            SupervisorLevel = ParseSupervisorLevel(rule.SupervisorLevel),
            FormFieldKey = rule.FormFieldKey,
        };
    }

    protected virtual IReadOnlyCollection<ApprovalActionDefinition> MapActions(
        IReadOnlyCollection<ApprovalDesignAction>? actions)
    {
        if (actions == null || actions.Count == 0)
        {
            return
            [
                new ApprovalActionDefinition { Key = ApprovalActionKey.Approve, Effect = ApprovalActionEffect.Approve },
                new ApprovalActionDefinition { Key = ApprovalActionKey.Reject, Effect = ApprovalActionEffect.Reject },
            ];
        }

        return actions.Select(action => new ApprovalActionDefinition
        {
            Key = Enum.TryParse<ApprovalActionKey>(action.Key, true, out var key) ? key : ApprovalActionKey.Approve,

            Effect = Enum.TryParse<ApprovalActionEffect>(action.Effect, true, out var effect)
                ? effect
                : ApprovalActionEffect.Approve,
        }).ToArray();
    }

    protected static string ToJavaScript(string expression)
    {
        // Designer expressions look like "form.amount > 5000"; expose the submitted form data as `form` and
        // evaluate defensively so a malformed expression routes to the default branch instead of faulting the workflow.
        return "(function(){try{var form=JSON.parse(getApprovalFormData()||'{}');return (" +
               expression +
               ");}catch(e){return false;}})()";
    }

    protected static ApprovalSignMode ParseSignMode(string? value)
    {
        return Enum.TryParse<ApprovalSignMode>(value, true, out var mode) ? mode : ApprovalSignMode.Any;
    }

    protected static EmptyApproverPolicy ParseEmptyPolicy(string? value)
    {
        return Enum.TryParse<EmptyApproverPolicy>(value, true, out var policy)
            ? policy
            : EmptyApproverPolicy.Skip;
    }

    protected static SupervisorLevelKind ParseSupervisorLevel(string? value)
    {
        if (string.Equals(value, "Nearest", StringComparison.OrdinalIgnoreCase))
        {
            return SupervisorLevelKind.Direct;
        }

        return Enum.TryParse<SupervisorLevelKind>(value, true, out var level)
            ? level
            : SupervisorLevelKind.Direct;
    }
}
