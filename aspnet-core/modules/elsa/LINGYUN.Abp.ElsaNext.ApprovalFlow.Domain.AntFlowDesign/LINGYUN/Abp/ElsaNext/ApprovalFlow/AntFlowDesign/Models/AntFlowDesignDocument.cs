using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// Wrapper used by AntFlow designs that are stored as <c>{ nodes: [ ... ] }</c>.
/// The array may either contain complete node trees (recursive <c>childNode</c>) or the
/// flattened list produced by the designer (<c>nodeFrom</c>/<c>nodeTo</c>).
/// </summary>
public sealed class AntFlowDesignDocument
{
    private List<AntFlowDesignNode> _nodes = [];

    public List<AntFlowDesignNode> Nodes
    {
        get => _nodes;
        set => _nodes = value ?? [];
    }
}
