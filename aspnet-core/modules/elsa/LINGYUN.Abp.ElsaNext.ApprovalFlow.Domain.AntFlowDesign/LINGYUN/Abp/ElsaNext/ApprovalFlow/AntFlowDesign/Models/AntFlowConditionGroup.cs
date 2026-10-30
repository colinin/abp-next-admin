using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

public sealed class AntFlowConditionGroup
{
    private List<AntFlowConditionItem> _conditionList = [];

    public string? GroupId { get; set; }

    public string? GroupName { get; set; }

    public bool CondRelation { get; set; } = true;

    public int? Sort { get; set; }

    public List<AntFlowConditionItem> ConditionList
    {
        get => _conditionList;
        set => _conditionList = value ?? [];
    }
}
