using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// One entry of <c>nodeProperty.assigneeList</c>. The designer writes <c>{ type, targetId, name }</c>;
/// other AntFlow versions use <c>id</c>/<c>userId</c>/<c>displayName</c>.
/// </summary>
public sealed class AntFlowAssignee
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public string? TargetId { get; set; }

    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    /// <summary>1: user, 3: role (AntFlow designer convention).</summary>
    public int? Type { get; set; }
}
