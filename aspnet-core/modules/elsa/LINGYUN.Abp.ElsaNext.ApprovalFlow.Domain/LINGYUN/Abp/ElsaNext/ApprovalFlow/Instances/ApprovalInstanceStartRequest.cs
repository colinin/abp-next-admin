using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public sealed record ApprovalInstanceStartRequest(Guid TemplateId, string Title, ParticipantReference? Initiator = null, string? BusinessKey = null, string? FormDataJson = null);
