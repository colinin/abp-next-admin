using Elsa.ApprovalFlow.Activities;
using Elsa.ApprovalFlow.Models;
using Elsa.Expressions.JavaScript.Models;
using Elsa.UserTasks.Models;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign;

/// <summary>
/// Compiles an AntFlow designer document into an Elsa <see cref="Sequence"/>. The AntFlow model is
/// independent from the legacy approval design document, so it lives in the domain layer.
/// </summary>
public class AntFlowDesignWorkflowTemplateCompiler : DomainService, IWorkflowTemplateCompiler
{
    /// <summary>
    /// Deserializes and compiles an AntFlow design. A design is either one node tree rooted at the
    /// start node or a <c>{ nodes: [ ... ] }</c> wrapper whose nodes are either complete trees or the
    /// flattened list produced by the AntFlow designer (<c>nodeFrom</c>/<c>nodeTo</c>).
    /// </summary>
    public virtual Sequence Compile(JsonElement rootElement, ApprovalTemplate template)
    {
        AntFlowDesignNode? root = null;
        IReadOnlyCollection<AntFlowDesignNode> nodes = [];
        try
        {
            if (TryGetJsonProperty(rootElement, "nodeType", out _))
            {
                // { nodeType: 1, childNode: { ... } } - a single AntFlow node tree.
                root = rootElement.Deserialize<AntFlowDesignNode>(AntFlowJson.Options);
            }
            else
            {
                // { nodes: [ ... ] } - either node trees or the designer's flattened node list.
                var antFlowDocument = rootElement.Deserialize<AntFlowDesignDocument>(AntFlowJson.Options);
                nodes = antFlowDocument?.Nodes ?? [];
            }
        }
        catch (JsonException exception)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.WorkflowInvalid, innerException: exception);
        }

        var tenantId = template.TenantId?.ToString("D") ?? "";
        var sequence = CompileAntFlowDesign(root, nodes, template, tenantId);

        if (sequence.Activities.Count == 0)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Template.WorkflowInvalid);
        }

        return sequence;
    }

    /// <summary>
    /// Compiles an AntFlow design. A design is either one node tree rooted at the start node or a
    /// <c>{ nodes: [ ... ] }</c> wrapper whose nodes are either complete trees or the flattened list
    /// produced by the AntFlow designer (<c>nodeFrom</c>/<c>nodeTo</c>).
    /// </summary>
    protected virtual Sequence CompileAntFlowDesign(
        AntFlowDesignNode? root,
        IReadOnlyCollection<AntFlowDesignNode>? nodes,
        ApprovalTemplate template,
        string tenantId)
    {
        var sequence = new Sequence();

        if (root != null)
        {
            CompileAntFlowChain(sequence, root, template, tenantId, NewAntFlowVisitSet());
            return sequence;
        }

        if (nodes == null || nodes.Count == 0)
        {
            return sequence;
        }

        foreach (var nodeRoot in ResolveAntFlowRoots(nodes))
        {
            CompileAntFlowChain(sequence, nodeRoot, template, tenantId, NewAntFlowVisitSet());
        }

        return sequence;
    }

    /// <summary>
    /// Walks one AntFlow chain. Start nodes are transparent, approval/cc nodes become Elsa activities,
    /// gateways become nested <see cref="If"/> chains and parallel gateways are linearised into the
    /// same approval tasks the legacy compiler produces.
    /// </summary>
    protected virtual void CompileAntFlowChain(
        Sequence sequence,
        AntFlowDesignNode? node,
        ApprovalTemplate template,
        string tenantId,
        HashSet<AntFlowDesignNode> visited)
    {
        while (node != null && visited.Add(node))
        {
            var property = GetAntFlowProperty(node);

            switch (GetAntFlowNodeType(node))
            {
                case AntFlowNodeTypes.ApproveNode:
                case AntFlowNodeTypes.ParallelApproveNode:
                    sequence.Activities.Add(BuildAntFlowApprovalTask(node, property, template, tenantId));
                    node = node.ChildNode;
                    break;
                case AntFlowNodeTypes.CopyNode:
                    sequence.Activities.Add(BuildAntFlowApprovalCc(node, property, template, tenantId));
                    node = node.ChildNode;
                    break;
                case AntFlowNodeTypes.GatewayNode:
                    if (property.ConditionNodes.Count > 0)
                    {
                        sequence.Activities.Add(BuildAntFlowCondition(node, property, template, tenantId));

                        // The condition branches own the merge/continuation tail in the AntFlow tree,
                        // so the gateway's child node must not be appended a second time.
                        node = null;
                    }
                    else
                    {
                        node = node.ChildNode;
                    }
                    break;
                case AntFlowNodeTypes.ConditionNode:
                    sequence.Activities.Add(BuildAntFlowSingleCondition(node, property, template, tenantId));
                    node = node.ChildNode;
                    break;
                case AntFlowNodeTypes.ParallelApproveWayNode:
                    var parallelNodes = property.ParallelNodes;
                    if (parallelNodes.Count > 0 ||
                        GetAntFlowNodeType(node.ChildNode) is AntFlowNodeTypes.ApproveNode or AntFlowNodeTypes.ParallelApproveNode)
                    {
                        sequence.Activities.Add(BuildAntFlowParallelWay(node, property, template, tenantId));

                        // The parallel gateway owns its child node (a parallel approver or the merge
                        // continuation), which is compiled as part of the parallel block.
                        node = null;
                    }
                    else
                    {
                        node = node.ChildNode;
                    }
                    break;
                default:
                    // StartNode and unknown/unsupported nodes only contribute their child chain.
                    node = node.ChildNode;
                    break;
            }
        }
    }

    /// <summary>
    /// Builds the nested If/Else chain of a gateway. Branches flagged with <c>isDefault == 1</c> are
    /// always the final Else and are never evaluated.
    /// </summary>
    protected virtual Activity BuildAntFlowCondition(
        AntFlowDesignNode gateway,
        AntFlowNodeProperty property,
        ApprovalTemplate template,
        string tenantId)
    {
        var conditionNodes = property.ConditionNodes;
        if (conditionNodes.Count == 0)
        {
            return CompileAntFlowBranch(gateway.ChildNode, template, tenantId);
        }

        var branches = new List<AntFlowConditionBranch>(conditionNodes.Count);
        foreach (var conditionNode in conditionNodes)
        {
            var conditionProperty = GetAntFlowProperty(conditionNode);
            branches.Add(new AntFlowConditionBranch(
                conditionNode,
                conditionProperty.IsDefault == 1,
                BuildAntFlowConditionExpression(conditionNode, conditionProperty)));
        }

        var defaultBranch = branches.LastOrDefault(branch => branch.IsDefault);
        var conditionalBranches = branches
            .Where(branch => !branch.IsDefault && !string.IsNullOrWhiteSpace(branch.Expression))
            .ToList();

        Activity BuildChain(int index)
        {
            if (index >= conditionalBranches.Count)
            {
                return defaultBranch == null
                    ? new Sequence()
                    : CompileAntFlowBranch(defaultBranch.Node.ChildNode, template, tenantId);
            }

            var branch = conditionalBranches[index];
            return new If
            {
                Condition = new Input<bool>(JavaScriptExpression.Create(ToJavaScript(branch.Expression!))),
                Then = CompileAntFlowBranch(branch.Node.ChildNode, template, tenantId),
                Else = BuildChain(index + 1),
            };
        }

        var conditionChain = BuildChain(0);

        // AntFlow keeps the continuation after the branch decision (typically the CC node)
        // in the gateway's ChildNode. Append it after the If so CC is sent only after the
        // approval branch has finished, not when the instance starts.
        if (gateway.ChildNode == null ||
            GetAntFlowNodeType(gateway.ChildNode) != AntFlowNodeTypes.CopyNode)
        {
            return conditionChain;
        }

        var sequence = new Sequence();
        sequence.Activities.Add(conditionChain);
        CompileAntFlowChain(sequence, gateway.ChildNode, template, tenantId, NewAntFlowVisitSet());
        return sequence;
    }

    /// <summary>
    /// Compiles a condition node that is not nested below a gateway. It still produces an
    /// <see cref="If"/> so the branch is only executed when the condition matches.
    /// </summary>
    protected virtual Activity BuildAntFlowSingleCondition(
        AntFlowDesignNode node,
        AntFlowNodeProperty property,
        ApprovalTemplate template,
        string tenantId)
    {
        var expression = BuildAntFlowConditionExpression(node, property);
        var then = CompileAntFlowBranch(node.ChildNode, template, tenantId);

        if (string.IsNullOrWhiteSpace(expression))
        {
            return then;
        }

        return new If
        {
            Condition = new Input<bool>(JavaScriptExpression.Create(ToJavaScript(expression))),
            Then = then,
            Else = new Sequence(),
        };
    }

    /// <summary>
    /// ParallelApproveWayNode is rendered as a sequence of ApprovalTask activities. The designer has no
    /// join gateway in this model, so the branches are linearised and the merge tail is not re-executed.
    /// </summary>
    protected virtual Sequence BuildAntFlowParallelWay(
        AntFlowDesignNode gateway,
        AntFlowNodeProperty property,
        ApprovalTemplate template,
        string tenantId)
    {
        var sequence = new Sequence();
        var parallelNodes = new List<AntFlowDesignNode>(property.ParallelNodes);

        // The designer also stores a parallel approver in childNode; it belongs to the same group.
        if (gateway.ChildNode != null &&
            GetAntFlowNodeType(gateway.ChildNode) is AntFlowNodeTypes.ApproveNode or AntFlowNodeTypes.ParallelApproveNode &&
            !IsAntFlowParallelMember(gateway.ChildNode, parallelNodes))
        {
            parallelNodes.Add(gateway.ChildNode);
        }

        // Shared by the branch tails so the merge node after the gateway is compiled (and executed)
        // only once even when every parallel branch points at it.
        var compiledTailNodes = new HashSet<AntFlowDesignNode>(AntFlowNodeReferenceComparer.Instance);

        foreach (var parallelNode in parallelNodes)
        {
            if (GetAntFlowNodeType(parallelNode) is AntFlowNodeTypes.ApproveNode or AntFlowNodeTypes.ParallelApproveNode)
            {
                sequence.Activities.Add(BuildAntFlowApprovalTask(
                    parallelNode,
                    GetAntFlowProperty(parallelNode),
                    template,
                    tenantId));
                compiledTailNodes.Add(parallelNode);
            }
            else
            {
                CompileAntFlowChain(sequence, parallelNode, template, tenantId, compiledTailNodes);
            }
        }

        // The continuation after the parallel approvals is stored on one of the parallel nodes.
        foreach (var parallelNode in parallelNodes)
        {
            if (parallelNode.ChildNode != null)
            {
                CompileAntFlowChain(sequence, parallelNode.ChildNode, template, tenantId, compiledTailNodes);
            }
        }

        // Some designs put the merge continuation directly on the gateway instead of a branch.
        if (gateway.ChildNode != null &&
            GetAntFlowNodeType(gateway.ChildNode) is not (AntFlowNodeTypes.ApproveNode or AntFlowNodeTypes.ParallelApproveNode))
        {
            CompileAntFlowChain(sequence, gateway.ChildNode, template, tenantId, compiledTailNodes);
        }

        return sequence;
    }

    protected virtual Sequence CompileAntFlowBranch(
        AntFlowDesignNode? node,
        ApprovalTemplate template,
        string tenantId)
    {
        var sequence = new Sequence();
        CompileAntFlowChain(sequence, node, template, tenantId, NewAntFlowVisitSet());
        return sequence;
    }

    protected virtual ApprovalTask BuildAntFlowApprovalTask(
        AntFlowDesignNode node,
        AntFlowNodeProperty property,
        ApprovalTemplate template,
        string tenantId)
    {
        var name = GetAntFlowNodeName(node, "审批节点");

        return new ApprovalTask
        {
            Title = new Input<string>(name),
            Summary = new Input<string?>(template.Description),
            ApprovalNodeKey = new Input<string?>(node.NodeId),
            ApprovalNodeName = new Input<string?>(name),
            ApproverRules = new Input<IReadOnlyCollection<ApproverRule>?>(BuildAntFlowRules(property, tenantId)),
            SignMode = new Input<ApprovalSignMode>(GetAntFlowSignMode(property.SignType)),
            Actions = new Input<IReadOnlyCollection<ApprovalActionDefinition>?>(MapAntFlowActions(property)),
            OnEmpty = new Input<EmptyApproverPolicy>(GetAntFlowEmptyPolicy(property.NoHeaderAction)),
        };
    }

    protected virtual ApprovalCc BuildAntFlowApprovalCc(
        AntFlowDesignNode node,
        AntFlowNodeProperty property,
        ApprovalTemplate template,
        string tenantId)
    {
        return new ApprovalCc
        {
            Title = new Input<string>(GetAntFlowNodeName(node, "抄送人")),
            Summary = new Input<string?>(template.Description),
            RecipientRules = new Input<IReadOnlyCollection<ApproverRule>?>(BuildAntFlowRules(property, tenantId, excludeInitiator: false)),
        };
    }

    /// <summary>
    /// Maps AntFlow's <c>assigneeList</c> to a single Participant rule. When the list is empty an
    /// empty Participant rule is emitted, matching the legacy compiler's behaviour for nodes without rules.
    /// </summary>
    protected virtual IReadOnlyCollection<ApproverRule> BuildAntFlowRules(
        AntFlowNodeProperty property,
        string tenantId,
        bool excludeInitiator = true)
    {
        var participants = new List<ParticipantReference>();

        foreach (var assignee in property.AssigneeList)
        {
            var id = FirstNonEmpty(assignee.Id, assignee.UserId, assignee.TargetId);
            if (string.IsNullOrWhiteSpace(id))
            {
                continue;
            }

            // The AntFlow designer uses 1 for users and 3 for roles; role assignment can also be
            // expressed through nodeProperty.assigneeType == 2.
            var isRole = assignee.Type == 3 || (assignee.Type == null && property.AssigneeType == 2);
            participants.Add(new ParticipantReference(
                tenantId,
                isRole ? ParticipantConsts.RoleProvider : ParticipantConsts.DefaultProvider,
                isRole ? UserTaskParticipantType.Group : UserTaskParticipantType.User,
                id,
                FirstNonEmpty(assignee.DisplayName, assignee.Name)));
        }

        if (participants.Count == 0)
        {
            // AntFlow stores supervisor/initiator/self-selection rules only as assigneeType.
            var implicitRule = property.AssigneeType switch
            {
                2 => new ApproverRule { Kind = ApproverKind.Role },
                3 => new ApproverRule { Kind = ApproverKind.Supervisor, SupervisorLevel = SupervisorLevelKind.Direct },
                4 => new ApproverRule { Kind = ApproverKind.OrganizationUnit },
                5 => new ApproverRule { Kind = ApproverKind.Initiator },
                6 => new ApproverRule
                {
                    Kind = ApproverKind.Supervisor,
                    SupervisorLevel = SupervisorLevelKind.LevelN,
                    Level = Math.Max(1, property.DirectorLevel ?? 1),
                },
                7 => new ApproverRule { Kind = ApproverKind.Supervisor, SupervisorLevel = SupervisorLevelKind.TopMost },
                8 => new ApproverRule { Kind = ApproverKind.InitiatorSelection },
                _ => new ApproverRule { Kind = ApproverKind.Participant },
            };
            implicitRule.ExcludeInitiator = excludeInitiator;
            return [implicitRule];
        }

        return
        [
            new ApproverRule
            {
                Kind = property.AssigneeType == 2 ? ApproverKind.Role : ApproverKind.Participant,
                Participants = participants,
                ExcludeInitiator = excludeInitiator,
            },
        ];
    }

    #region AntFlow condition expressions

    /// <summary>
    /// Builds the JavaScript expression of one condition branch. A legacy <c>conditionExpression</c>
    /// or <c>expression</c> string always wins over the structured condition groups.
    /// </summary>
    protected virtual string? BuildAntFlowConditionExpression(
        AntFlowDesignNode node,
        AntFlowNodeProperty property)
    {
        var legacyExpression = FirstNonEmpty(
            property.ConditionExpression,
            property.Expression,
            node.ConditionExpression,
            node.Expression);
        if (!string.IsNullOrWhiteSpace(legacyExpression))
        {
            return legacyExpression;
        }

        var groups = new List<AntFlowConditionGroup>();
        if (property.GroupConditions.Count > 0)
        {
            groups.AddRange(property.GroupConditions);
        }
        else if (property.ConditionList.Count > 0)
        {
            groups.Add(new AntFlowConditionGroup
            {
                CondRelation = property.CondRelation,
                ConditionList = property.ConditionList,
            });
        }

        var groupExpressions = new List<string>();
        foreach (var group in groups)
        {
            var groupExpression = BuildAntFlowConditionGroupExpression(group);
            if (!string.IsNullOrWhiteSpace(groupExpression))
            {
                groupExpressions.Add(groupExpression);
            }
        }

        if (groupExpressions.Count == 0)
        {
            return null;
        }

        if (groupExpressions.Count == 1)
        {
            return groupExpressions[0];
        }

        // true = 且 (AND), false = 或 (OR) - see the AntFlow designer constants.
        var groupJoiner = property.GroupRelation ? " && " : " || ";
        return string.Join(groupJoiner, groupExpressions.Select(expression => $"({expression})"));
    }

    protected virtual string? BuildAntFlowConditionGroupExpression(AntFlowConditionGroup group)
    {
        var conditionExpressions = new List<string>();
        foreach (var condition in group.ConditionList)
        {
            var conditionExpression = BuildAntFlowConditionItemExpression(condition);
            if (!string.IsNullOrWhiteSpace(conditionExpression))
            {
                conditionExpressions.Add(conditionExpression);
            }
        }

        if (conditionExpressions.Count == 0)
        {
            return null;
        }

        if (conditionExpressions.Count == 1)
        {
            return conditionExpressions[0];
        }

        // true = 且 (AND), false = 或 (OR) inside a condition group.
        var conditionJoiner = group.CondRelation ? " && " : " || ";
        return string.Join(conditionJoiner, conditionExpressions.Select(expression => $"({expression})"));
    }

    protected virtual string? BuildAntFlowConditionItemExpression(AntFlowConditionItem condition)
    {
        var fieldKey = FirstNonEmpty(condition.Key, condition.ColumnDbName);
        if (string.IsNullOrWhiteSpace(fieldKey))
        {
            return null;
        }

        var operatorText = NormalizeAntFlowOperator(condition.OptType);
        if (operatorText == null)
        {
            return null;
        }

        var fieldAccess = ToAntFlowFormAccess(fieldKey);

        if (operatorText == "between")
        {
            var lower = ToAntFlowJavaScriptValue(condition.Zdy1, condition.Type);
            var upper = ToAntFlowJavaScriptValue(condition.Zdy2, condition.Type);

            if (lower == null || upper == null)
            {
                // Some designs place a two element array in zdy1 instead of filling zdy2.
                var values = ToAntFlowJavaScriptArray(condition.Zdy1, condition.Type);
                if (values == null || values.Count < 2)
                {
                    return null;
                }

                return $"({fieldAccess} >= {values[0]} && {fieldAccess} <= {values[1]})";
            }

            return $"({fieldAccess} >= {lower} && {fieldAccess} <= {upper})";
        }

        if (operatorText is "in" or "notin")
        {
            var values = ToAntFlowJavaScriptArray(condition.Zdy1, condition.Type)
                         ?? ToAntFlowJavaScriptArray(condition.Zdy2, condition.Type);
            if (values == null || values.Count == 0)
            {
                return null;
            }

            var arrayLiteral = "[" + string.Join(", ", values) + "]";
            return operatorText == "in"
                ? $"({arrayLiteral}.indexOf({fieldAccess}) >= 0)"
                : $"({arrayLiteral}.indexOf({fieldAccess}) < 0)";
        }

        var value = ToAntFlowJavaScriptValue(condition.Zdy1, condition.Type);
        if (value == null)
        {
            return null;
        }

        return $"{fieldAccess} {operatorText} {value}";
    }

    /// <summary>
    /// AntFlow stores the operator either as 1..6 (1 小于, 2 大于, 3 小于等于, 4 等于, 5 大于等于,
    /// 6 介于) or as a symbolic string.
    /// </summary>
    protected static string? NormalizeAntFlowOperator(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "==";
        }

        return value.Trim().ToLowerInvariant() switch
        {
            "1" => "<",
            "2" => ">",
            "3" => "<=",
            "4" => "==",
            "5" => ">=",
            "6" => "between",
            "<" or "lt" => "<",
            ">" or "gt" => ">",
            "<=" or "≤" or "lte" or "le" => "<=",
            ">=" or "≥" or "gte" or "ge" => ">=",
            "=" or "==" or "===" or "eq" or "equals" => "==",
            "!=" or "<>" or "neq" or "ne" => "!=",
            "between" => "between",
            "in" => "in",
            "notin" or "not in" => "notin",
            _ => null,
        };
    }

    protected static string ToAntFlowFormAccess(string fieldKey)
    {
        if (fieldKey.StartsWith("form.", StringComparison.OrdinalIgnoreCase))
        {
            return fieldKey;
        }

        return IsAntFlowJavaScriptIdentifier(fieldKey)
            ? $"form.{fieldKey}"
            : $"form[{JsonSerializer.Serialize(fieldKey)}]";
    }

    protected static bool IsAntFlowJavaScriptIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var first = value[0];
        if (!char.IsLetter(first) && first != '_' && first != '$')
        {
            return false;
        }

        for (var index = 1; index < value.Length; index++)
        {
            var character = value[index];
            if (!char.IsLetterOrDigit(character) && character != '_' && character != '$')
            {
                return false;
            }
        }

        return true;
    }

    protected static string? ToAntFlowJavaScriptValue(JsonElement? value, string? type)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var element = value.Value;
        switch (element.ValueKind)
        {
            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                return null;
            case JsonValueKind.String:
                var text = element.GetString();
                return string.IsNullOrWhiteSpace(text) ? null : FormatAntFlowStringValue(text, type);
            case JsonValueKind.Number:
                return element.GetRawText();
            case JsonValueKind.True:
                return "true";
            case JsonValueKind.False:
                return "false";
            case JsonValueKind.Array:
                var values = ToAntFlowJavaScriptArray(element, type);
                return values == null ? null : "[" + string.Join(", ", values) + "]";
            default:
                return element.GetRawText();
        }
    }

    protected static string FormatAntFlowStringValue(string text, string? type)
    {
        if (string.Equals(type, "number", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "int", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "integer", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "double", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "decimal", StringComparison.OrdinalIgnoreCase))
        {
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var numeric))
            {
                return numeric.ToString(CultureInfo.InvariantCulture);
            }
        }

        if (string.Equals(type, "switch", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "boolean", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(type, "bool", StringComparison.OrdinalIgnoreCase))
        {
            if (bool.TryParse(text, out var boolean))
            {
                return boolean ? "true" : "false";
            }

            if (text == "1")
            {
                return "true";
            }

            if (text == "0")
            {
                return "false";
            }
        }

        // JsonSerializer produces a double quoted string with JavaScript compatible escapes.
        return JsonSerializer.Serialize(text);
    }

    protected static IReadOnlyList<string>? ToAntFlowJavaScriptArray(JsonElement? value, string? type)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var element = value.Value;
        if (element.ValueKind == JsonValueKind.Array)
        {
            var items = new List<string>();
            foreach (var item in element.EnumerateArray())
            {
                var literal = ToAntFlowJavaScriptValue(item, type);
                if (literal != null)
                {
                    items.Add(literal);
                }
            }

            return items;
        }

        if (element.ValueKind == JsonValueKind.String)
        {
            var text = element.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var trimmed = text.Trim();
            if (trimmed.StartsWith("[", StringComparison.Ordinal))
            {
                try
                {
                    using var document = JsonDocument.Parse(trimmed);
                    if (document.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        return ToAntFlowJavaScriptArray(document.RootElement, type);
                    }
                }
                catch (JsonException)
                {
                    // Not a JSON array; fall back to the comma separated form.
                }
            }

            return trimmed
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Select(item => FormatAntFlowStringValue(item, type))
                .ToList();
        }

        if (element.ValueKind == JsonValueKind.Number)
        {
            return [element.GetRawText()];
        }

        return null;
    }

    #endregion

    /// <summary>
    /// Resolves the top level AntFlow nodes. A flattened designer payload is rebuilt into a tree first.
    /// </summary>
    protected virtual IReadOnlyCollection<AntFlowDesignNode> ResolveAntFlowRoots(
        IReadOnlyCollection<AntFlowDesignNode> nodes)
    {
        var hasTreeStructure = false;
        var hasLinks = false;

        foreach (var node in nodes)
        {
            if (node.ChildNode != null)
            {
                hasTreeStructure = true;
            }

            var property = GetAntFlowProperty(node);
            if (property.ConditionNodes.Count > 0 || property.ParallelNodes.Count > 0)
            {
                hasTreeStructure = true;
            }

            if (node.NodeTo.Count > 0)
            {
                hasLinks = true;
            }
        }

        return hasTreeStructure || !hasLinks
            ? FindAntFlowTreeRoots(nodes)
            : BuildAntFlowRootsFromFlat(nodes);
    }

    protected virtual IReadOnlyCollection<AntFlowDesignNode> FindAntFlowTreeRoots(
        IReadOnlyCollection<AntFlowDesignNode> nodes)
    {
        var referenced = new HashSet<AntFlowDesignNode>(AntFlowNodeReferenceComparer.Instance);

        foreach (var node in nodes)
        {
            MarkAntFlowChildren(node, referenced, new HashSet<AntFlowDesignNode>(AntFlowNodeReferenceComparer.Instance));
        }

        var roots = nodes.Where(node => !referenced.Contains(node)).ToList();
        if (roots.Count == 0)
        {
            roots = nodes.ToList();
        }

        MoveAntFlowStartNodeFirst(roots);
        return roots;
    }

    protected static void MarkAntFlowChildren(
        AntFlowDesignNode node,
        ISet<AntFlowDesignNode> referenced,
        ISet<AntFlowDesignNode> visited)
    {
        if (!visited.Add(node))
        {
            return;
        }

        if (node.ChildNode != null && referenced.Add(node.ChildNode))
        {
            MarkAntFlowChildren(node.ChildNode, referenced, visited);
        }

        var property = GetAntFlowProperty(node);
        foreach (var condition in property.ConditionNodes)
        {
            if (referenced.Add(condition))
            {
                MarkAntFlowChildren(condition, referenced, visited);
            }
        }

        foreach (var parallel in property.ParallelNodes)
        {
            if (referenced.Add(parallel))
            {
                MarkAntFlowChildren(parallel, referenced, visited);
            }
        }
    }

    /// <summary>
    /// Rebuilds the recursive <c>childNode</c> tree from the flattened designer list
    /// (<c>nodeFrom</c>/<c>nodeTo</c> plus gateway <c>conditionNodes</c>/<c>parallelNodes</c>).
    /// </summary>
    protected virtual IReadOnlyCollection<AntFlowDesignNode> BuildAntFlowRootsFromFlat(
        IReadOnlyCollection<AntFlowDesignNode> nodes)
    {
        var byId = new Dictionary<string, AntFlowDesignNode>(StringComparer.OrdinalIgnoreCase);
        foreach (var node in nodes)
        {
            if (!string.IsNullOrWhiteSpace(node.NodeId) && !byId.ContainsKey(node.NodeId))
            {
                byId[node.NodeId] = node;
            }
        }

        var linked = new HashSet<AntFlowDesignNode>(AntFlowNodeReferenceComparer.Instance);
        foreach (var node in nodes)
        {
            LinkAntFlowNode(node, byId, linked);
        }

        var targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var node in nodes)
        {
            foreach (var targetId in node.NodeTo)
            {
                if (!string.IsNullOrWhiteSpace(targetId))
                {
                    targets.Add(targetId);
                }
            }
        }

        var roots = nodes
            .Where(node => !string.IsNullOrWhiteSpace(node.NodeId) && !targets.Contains(node.NodeId))
            .ToList();

        if (roots.Count == 0)
        {
            roots = nodes
                .Where(node => GetAntFlowNodeType(node) == AntFlowNodeTypes.StartNode)
                .ToList();
        }

        if (roots.Count == 0)
        {
            roots = nodes.ToList();
        }

        MoveAntFlowStartNodeFirst(roots);
        return roots;
    }

    protected static void LinkAntFlowNode(
        AntFlowDesignNode node,
        IReadOnlyDictionary<string, AntFlowDesignNode> byId,
        ISet<AntFlowDesignNode> linked)
    {
        if (!linked.Add(node))
        {
            return;
        }

        node.NodeProperty ??= new AntFlowNodeProperty();
        var property = node.NodeProperty;

        var targets = new List<AntFlowDesignNode>();
        foreach (var targetId in node.NodeTo)
        {
            if (!string.IsNullOrWhiteSpace(targetId) &&
                byId.TryGetValue(targetId, out var target) &&
                !ReferenceEquals(target, node))
            {
                targets.Add(target);
            }
        }

        switch (GetAntFlowNodeType(node))
        {
            case AntFlowNodeTypes.GatewayNode:
                var conditions = targets
                    .Where(target => GetAntFlowNodeType(target) == AntFlowNodeTypes.ConditionNode)
                    .ToList();
                if (conditions.Count > 0)
                {
                    property.ConditionNodes = conditions;
                }

                node.ChildNode ??= targets.FirstOrDefault(target => GetAntFlowNodeType(target) != AntFlowNodeTypes.ConditionNode);

                foreach (var condition in property.ConditionNodes)
                {
                    LinkAntFlowNode(condition, byId, linked);
                }
                break;
            case AntFlowNodeTypes.ParallelApproveWayNode:
                var parallelNodes = targets
                    .Where(target => GetAntFlowNodeType(target) == AntFlowNodeTypes.ParallelApproveNode)
                    .ToList();
                if (parallelNodes.Count > 0)
                {
                    property.ParallelNodes = parallelNodes;
                }

                node.ChildNode ??= targets.FirstOrDefault(target => GetAntFlowNodeType(target) != AntFlowNodeTypes.ParallelApproveNode);

                foreach (var parallelNode in property.ParallelNodes)
                {
                    LinkAntFlowNode(parallelNode, byId, linked);
                }
                break;
            default:
                node.ChildNode ??= targets.FirstOrDefault();
                break;
        }

        if (node.ChildNode != null)
        {
            LinkAntFlowNode(node.ChildNode, byId, linked);
        }
    }

    protected static void MoveAntFlowStartNodeFirst(List<AntFlowDesignNode> roots)
    {
        var startIndex = roots.FindIndex(node => GetAntFlowNodeType(node) == AntFlowNodeTypes.StartNode);
        if (startIndex > 0)
        {
            var start = roots[startIndex];
            roots.RemoveAt(startIndex);
            roots.Insert(0, start);
        }
    }

    protected static bool IsAntFlowParallelMember(
        AntFlowDesignNode? child,
        IReadOnlyCollection<AntFlowDesignNode> parallelNodes)
    {
        if (child == null)
        {
            return false;
        }

        foreach (var parallelNode in parallelNodes)
        {
            if (ReferenceEquals(child, parallelNode) ||
                (!string.IsNullOrWhiteSpace(child.NodeId) &&
                 string.Equals(child.NodeId, parallelNode.NodeId, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }

        return false;
    }

    protected static HashSet<AntFlowDesignNode> NewAntFlowVisitSet()
        => new(AntFlowNodeReferenceComparer.Instance);

    protected static AntFlowNodeProperty GetAntFlowProperty(AntFlowDesignNode node)
        => node.NodeProperty ?? new AntFlowNodeProperty();

    protected static int GetAntFlowNodeType(AntFlowDesignNode? node)
        => node?.NodeType ?? 0;

    protected static string GetAntFlowNodeName(AntFlowDesignNode node, string fallback)
        => FirstNonEmpty(node.NodeDisplayName, node.NodeName) ?? fallback;

    protected static ApprovalSignMode GetAntFlowSignMode(int? signType)
        => signType switch
        {
            2 => ApprovalSignMode.All,
            3 => ApprovalSignMode.Sequential,
            _ => ApprovalSignMode.Any,
        };

    protected static EmptyApproverPolicy GetAntFlowEmptyPolicy(int? noHeaderAction)
        => noHeaderAction switch
        {
            0 => EmptyApproverPolicy.Fail,
            2 => EmptyApproverPolicy.EscalateToManager,
            _ => EmptyApproverPolicy.Skip,
        };

    protected static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }

    /// <summary>
    /// Detects the AntFlow model from the raw document before deserialization. Only numeric
    /// <c>nodeType</c> values are treated as AntFlow, so the legacy <c>{ nodes: [{ id, kind }] }</c>
    /// JSON can never be misrouted.
    /// </summary>
    public virtual bool CanCompile(JsonElement rootElement)
    {
        if (rootElement.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (TryGetJsonProperty(rootElement, "nodeType", out var nodeType) &&
            nodeType.ValueKind == JsonValueKind.Number &&
            nodeType.TryGetInt32(out _))
        {
            return true;
        }

        if (TryGetJsonProperty(rootElement, "nodes", out var nodes) &&
            nodes.ValueKind == JsonValueKind.Array)
        {
            foreach (var node in nodes.EnumerateArray())
            {
                if (node.ValueKind == JsonValueKind.Object &&
                    TryGetJsonProperty(node, "nodeType", out var childNodeType) &&
                    childNodeType.ValueKind == JsonValueKind.Number &&
                    childNodeType.TryGetInt32(out _))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected static bool TryGetJsonProperty(
        JsonElement element,
        string propertyName,
        out JsonElement value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            if (element.TryGetProperty(propertyName, out value))
            {
                return true;
            }

            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    protected virtual IReadOnlyCollection<ApprovalActionDefinition> MapAntFlowActions(
        AntFlowNodeProperty property)
    {
        var selected = property.Buttons != null &&
            property.Buttons.TryGetValue("approvalPage", out var values)
                ? values
                : null;

        if (selected == null || selected.Count == 0)
        {
            return
            [
                new ApprovalActionDefinition { Key = ApprovalActionKey.Approve, Effect = ApprovalActionEffect.Approve },
                new ApprovalActionDefinition { Key = ApprovalActionKey.Reject, Effect = ApprovalActionEffect.Reject },
            ];
        }

        var actions = new List<ApprovalActionDefinition>();
        foreach (var value in selected.Distinct())
        {
            switch (value)
            {
                case 3:
                    actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.Approve, Effect = ApprovalActionEffect.Approve });
                    break;
                case 4:
                    actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.Reject, Effect = ApprovalActionEffect.Reject });
                    break;
                case 18:
                    actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.Return, Effect = ApprovalActionEffect.Neutral });
                    break;
                case 21:
                    actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.Transfer, Effect = ApprovalActionEffect.Neutral });
                    break;
                case 19:
                    actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.AddApproval, Effect = ApprovalActionEffect.Neutral });
                    break;
            }
        }

        // Agree/reject are the minimum workflow controls and are always available.
        if (actions.All(a => a.Key != ApprovalActionKey.Approve))
        {
            actions.Insert(0, new ApprovalActionDefinition { Key = ApprovalActionKey.Approve, Effect = ApprovalActionEffect.Approve });
        }
        if (actions.All(a => a.Key != ApprovalActionKey.Reject))
        {
            actions.Add(new ApprovalActionDefinition { Key = ApprovalActionKey.Reject, Effect = ApprovalActionEffect.Reject });
        }

        return actions;
    }

    protected static string ToJavaScript(string expression)
    {
        // Designer expressions look like "form.amount > 5000"; expose the submitted form data as `form` and
        // evaluate defensively so a malformed expression routes to the default branch instead of faulting the workflow.
        return "(function(){try{var form=JSON.parse(getApprovalFormData()||'{}');return (" +
               expression +
               ");}catch(e){return false;}})()";
    }

    private sealed class AntFlowConditionBranch
    {
        public AntFlowConditionBranch(
            AntFlowDesignNode node,
            bool isDefault,
            string? expression)
        {
            Node = node;
            IsDefault = isDefault;
            Expression = expression;
        }

        public AntFlowDesignNode Node { get; }

        public bool IsDefault { get; }

        public string? Expression { get; }
    }
}
