using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// AntFlow node type identifiers (see the designer's <c>constant.js</c>).
/// </summary>
public static class AntFlowNodeKinds
{
    public const string Approval = "approval";

    public const string Condition = "condition";
}
