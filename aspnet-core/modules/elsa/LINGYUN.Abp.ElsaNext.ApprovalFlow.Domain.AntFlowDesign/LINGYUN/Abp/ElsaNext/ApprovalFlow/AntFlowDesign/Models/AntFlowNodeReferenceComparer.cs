using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

/// <summary>
/// Reference equality comparer used while walking the designer tree, which is a DAG when the
/// designer reuses the merge node after a gateway.
/// </summary>
internal sealed class AntFlowNodeReferenceComparer : IEqualityComparer<AntFlowDesignNode>
{
    public static readonly AntFlowNodeReferenceComparer Instance = new();

    public bool Equals(AntFlowDesignNode? x, AntFlowDesignNode? y) => ReferenceEquals(x, y);

    public int GetHashCode(AntFlowDesignNode obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
}
