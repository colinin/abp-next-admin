using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Services;

public sealed class BuiltInApproverRuleResolver : IApproverRuleResolver
{
	public IReadOnlySet<ApproverKind> SupportedKinds { get; } = new HashSet<ApproverKind>
	{
		ApproverKind.Participant,
		ApproverKind.Initiator,
		ApproverKind.InitiatorSelection,
		ApproverKind.FormField
	};

	public Task<IReadOnlyCollection<ParticipantReference>> ResolveAsync(ApproverRule rule, ApproverResolutionContext context, CancellationToken cancellationToken = default)
	{
		var participants = rule.Kind switch
		{
			ApproverKind.Participant => rule.Participants.ToArray(),
			ApproverKind.Initiator => context.Initiator == null
				? Array.Empty<ParticipantReference>()
				: new[] { context.Initiator },
			ApproverKind.InitiatorSelection => context.PreSelectedParticipants ?? Array.Empty<ParticipantReference>(),
			ApproverKind.FormField => ResolveFormField(rule, context),
			_ => Array.Empty<ParticipantReference>(),
		};

		return Task.FromResult(participants);
	}
	private static IReadOnlyCollection<ParticipantReference> ResolveFormField(ApproverRule rule, ApproverResolutionContext context)
	{
		if (!string.IsNullOrWhiteSpace(rule.FormFieldKey))
		{
			var formData = context.FormData;
			if (formData.HasValue)
			{
				var valueOrDefault = formData.GetValueOrDefault();
				if (valueOrDefault.ValueKind == JsonValueKind.Object && valueOrDefault.TryGetProperty(rule.FormFieldKey, out var value))
				{
					var list = new List<ParticipantReference>();
					switch (value.ValueKind)
					{
					case JsonValueKind.String:
						AddSubject(list, rule, context, value.GetString(), null, null);
						break;
					case JsonValueKind.Array:
						foreach (var item in value.EnumerateArray())
						{
							if (item.ValueKind == JsonValueKind.String)
							{
								AddSubject(list, rule, context, item.GetString(), null, null);
							}
							else if (item.ValueKind == JsonValueKind.Object)
							{
								AddSubject(list, rule, context, item.TryGetProperty("id", out var value5) ? value5.GetString() : null, item.TryGetProperty("provider", out var value6) ? value6.GetString() : null, item.TryGetProperty("type", out var value7) ? value7.GetString() : null);
							}
						}
						break;
					case JsonValueKind.Object:
					{
						AddSubject(list, rule, context, value.TryGetProperty("id", out var value2) ? value2.GetString() : null, value.TryGetProperty("provider", out var value3) ? value3.GetString() : null, value.TryGetProperty("type", out var value4) ? value4.GetString() : null);
						break;
					}
					}
					return list;
				}
			}
		}
		return Array.Empty<ParticipantReference>();
	}

	private static void AddSubject(ICollection<ParticipantReference> references, ApproverRule rule, ApproverResolutionContext context, string? id, string? provider, string? type)
	{
		if (!string.IsNullOrWhiteSpace(id))
		{
			var text = ((!string.IsNullOrWhiteSpace(provider)) ? provider : (context.Initiator?.Provider ?? rule.Participants.FirstOrDefault()?.Provider));
			if (!string.IsNullOrWhiteSpace(text))
			{
				references.Add(new ParticipantReference(context.Initiator?.TenantId ?? rule.Participants.FirstOrDefault()?.TenantId ?? context.TenantId, text, string.Equals(type, ParticipantConsts.GroupType, StringComparison.OrdinalIgnoreCase) ? UserTaskParticipantType.Group : UserTaskParticipantType.User, id));
			}
		}
	}
}
