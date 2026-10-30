using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

public static class AntFlowNodeTypes
{
    public const int StartNode = 1;

    public const int GatewayNode = 2;

    public const int ConditionNode = 3;

    public const int ApproveNode = 4;

    public const int ParallelApproveWayNode = 5;

    public const int CopyNode = 6;

    public const int ParallelApproveNode = 7;
}
