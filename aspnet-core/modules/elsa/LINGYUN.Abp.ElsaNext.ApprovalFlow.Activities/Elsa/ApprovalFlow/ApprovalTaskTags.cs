using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elsa.ApprovalFlow;

public static class ApprovalTaskTags
{
	public const string NodeInstancePrefix = "approval-node:";

	public const string SignModePrefix = "approval-sign-mode:";

	public const string AssigneePrefix = "approval-assignee:";

	public const string ActionEffectsPrefix = "approval-action-effects:";

	public const string OriginPrefix = "approval-origin:";

	public const string NodeIdPrefix = "approval-node-id:";

	public const string NodeNamePrefix = "approval-node-name:";

	public const string OriginWorkflow = "Workflow";

	public const string OriginAdded = "Added";

	public static string NodeInstance(string nodeInstanceId)
	{
		return NodeInstancePrefix + nodeInstanceId;
	}

	public static string SignMode(ApprovalSignMode signMode)
	{
		return SignModePrefix + signMode;
	}

	public static string Assignee(ParticipantReference assignee)
	{
		return AssigneePrefix + assignee.Provider + ":" + assignee.Id;
	}

	public static string NodeId(string nodeId)
	{
		return NodeIdPrefix + nodeId;
	}

	public static string NodeName(string nodeName)
	{
		return NodeNamePrefix + nodeName;
	}

	public static string? ReadNodeId(IEnumerable<string>? tags)
	{
		var text = Read(tags, NodeIdPrefix);
		return string.IsNullOrWhiteSpace(text) ? null : text;
	}

	public static string? ReadNodeName(IEnumerable<string>? tags)
	{
		var text = Read(tags, NodeNamePrefix);
		return string.IsNullOrWhiteSpace(text) ? null : text;
	}

	public static string Origin(string origin)
	{
		return OriginPrefix + origin;
	}

	public static string? ReadOrigin(IEnumerable<string>? tags)
	{
		var text = Read(tags, OriginPrefix);
		return string.IsNullOrWhiteSpace(text) ? null : text;
	}

	public static string? ReadNodeInstance(IEnumerable<string>? tags)
	{
		var text = Read(tags, NodeInstancePrefix);
		return string.IsNullOrWhiteSpace(text) ? null : text;
	}

	public static ApprovalSignMode? ReadSignMode(IEnumerable<string>? tags)
	{
		var value = Read(tags, SignModePrefix);
		ApprovalSignMode result;
		return Enum.TryParse(value, ignoreCase: true, out result) ? new ApprovalSignMode?(result) : ((ApprovalSignMode?)null);
	}

	public static string ActionEffects(IEnumerable<ApprovalActionDefinition>? actions)
	{
		return ActionEffectsPrefix + string.Join(",", 
            from x in actions ?? Array.Empty<ApprovalActionDefinition>()
			select $"{x.Key}={x.Effect}");
	}

	public static IReadOnlyDictionary<string, ApprovalActionEffect> ReadActionEffects(IEnumerable<string>? tags)
	{
		var text = Read(tags, ActionEffectsPrefix);
		if (string.IsNullOrWhiteSpace(text))
		{
			return new Dictionary<string, ApprovalActionEffect>(StringComparer.OrdinalIgnoreCase);
		}
		var dictionary = new Dictionary<string, ApprovalActionEffect>(StringComparer.OrdinalIgnoreCase);
		var array = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
		foreach (var text2 in array)
		{
			var num = text2.LastIndexOf('=');
			if (num > 0)
			{
				var key = text2.Substring(0, num);
				if (Enum.TryParse<ApprovalActionEffect>(text2.Substring(num + 1), ignoreCase: true, out var result))
				{
					dictionary[key] = result;
				}
			}
		}
		return dictionary;
	}

	private static string? Read(IEnumerable<string>? tags, string prefix)
	{
		return tags?.FirstOrDefault((string tag) => tag.StartsWith(prefix, StringComparison.Ordinal))?.Substring(prefix.Length);
	}
}
