using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// The AntFlow <c>nodeProperty</c> payload. The designer sometimes serialises it as a JSON string,
/// which is handled by <see cref="AntFlowNodePropertyConverter"/>.
/// </summary>
public sealed class AntFlowNodeProperty
{
    private List<AntFlowDesignNode> _conditionNodes = [];
    private List<AntFlowDesignNode> _parallelNodes = [];
    private List<AntFlowAssignee> _assigneeList = [];
    private List<AntFlowConditionGroup> _groupConditions = [];
    private List<AntFlowConditionItem> _conditionList = [];

    public List<AntFlowDesignNode> ConditionNodes
    {
        get => _conditionNodes;
        set => _conditionNodes = value ?? [];
    }

    public List<AntFlowDesignNode> ParallelNodes
    {
        get => _parallelNodes;
        set => _parallelNodes = value ?? [];
    }

    public List<AntFlowAssignee> AssigneeList
    {
        get => _assigneeList;
        set => _assigneeList = value ?? [];
    }

    /// <summary>1:指定成员, 2:指定角色, 3:直属领导, 5:发起人自己, 6:指定层级, 7:层层审批, 8:发起人自选.</summary>
    public int? AssigneeType { get; set; }

    /// <summary>1:或签, 2:会签, 3:顺序会签.</summary>
    public int? SignType { get; set; }

    /// <summary>AntFlow button permissions, e.g. { "approvalPage": [3,4,18] }.</summary>
    public Dictionary<string, List<int>>? Buttons { get; set; }

    /// <summary>0:不允许发起, 1:跳过, 2:转交给审核管理员.</summary>
    public int? NoHeaderAction { get; set; }

    public int? DirectorLevel { get; set; }

    /// <summary>1 marks the default (else) condition branch.</summary>
    public int? IsDefault { get; set; }

    /// <summary>true: 且 (AND) across groups; false: 或 (OR).</summary>
    public bool GroupRelation { get; set; }

    /// <summary>true: 且 (AND) inside a group; false: 或 (OR).</summary>
    public bool CondRelation { get; set; } = true;

    public List<AntFlowConditionGroup> GroupConditions
    {
        get => _groupConditions;
        set => _groupConditions = value ?? [];
    }

    /// <summary>A flat condition list used by older AntFlow payloads instead of <see cref="GroupConditions"/>.</summary>
    public List<AntFlowConditionItem> ConditionList
    {
        get => _conditionList;
        set => _conditionList = value ?? [];
    }

    public string? ConditionExpression { get; set; }

    public string? Expression { get; set; }
}
