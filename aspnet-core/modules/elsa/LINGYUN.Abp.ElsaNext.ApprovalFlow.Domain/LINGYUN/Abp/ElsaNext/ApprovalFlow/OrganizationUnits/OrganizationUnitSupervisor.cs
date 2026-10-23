using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnits;

public sealed record OrganizationUnitSupervisor(Guid OrganizationUnitId, string OrganizationUnitDisplayName, int Level, IReadOnlyList<Guid> ManagerUserIds);
