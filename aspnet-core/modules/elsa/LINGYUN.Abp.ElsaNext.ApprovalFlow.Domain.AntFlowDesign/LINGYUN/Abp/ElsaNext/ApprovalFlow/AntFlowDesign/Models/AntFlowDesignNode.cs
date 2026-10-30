using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// One AntFlow designer node. The designer keeps the tree in <see cref="ChildNode"/> and the
/// gateway branches in <see cref="NodeProperty"/>.
/// </summary>
public sealed class AntFlowDesignNode
{
    private List<string> _nodeTo = [];

    public string? NodeId { get; set; }

    public string? NodeName { get; set; }

    public string? NodeDisplayName { get; set; }

    public int? NodeType { get; set; }

    public string? NodeFrom { get; set; }

    public List<string> NodeTo
    {
        get => _nodeTo;
        set => _nodeTo = value ?? [];
    }

    public int? NodeWeight { get; set; }

    [JsonConverter(typeof(AntFlowNodePropertyConverter))]
    public AntFlowNodeProperty? NodeProperty { get; set; }

    public AntFlowDesignNode? ChildNode { get; set; }

    public bool? Error { get; set; }

    /// <summary>
    /// Some AntFlow versions keep a pre-built expression on the node instead of a condition group.
    /// It is honoured verbatim by the compiler.
    /// </summary>
    public string? ConditionExpression { get; set; }

    public string? Expression { get; set; }
}
