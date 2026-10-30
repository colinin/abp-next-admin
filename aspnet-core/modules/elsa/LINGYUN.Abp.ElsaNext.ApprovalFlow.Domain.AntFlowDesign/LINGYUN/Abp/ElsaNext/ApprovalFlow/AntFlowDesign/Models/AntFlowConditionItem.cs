using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign.Models;

public sealed class AntFlowConditionItem
{
    private JsonElement? _zdy1;
    private JsonElement? _zdy2;

    public string? Key { get; set; }

    public string? Label { get; set; }

    public string? Type { get; set; }

    /// <summary>Either the numeric designer operator (1..6) or a symbolic operator (<c>=</c>, <c>&gt;</c>, ...).</summary>
    public string? OptType { get; set; }

    public string? Opt1 { get; set; }

    public string? Opt2 { get; set; }

    public JsonElement? Zdy1
    {
        get => _zdy1;
        set => _zdy1 = value;
    }

    public JsonElement? Zdy2
    {
        get => _zdy2;
        set => _zdy2 = value;
    }

    public string? ColumnDbName { get; set; }

    public string? ColumnType { get; set; }

    public string? ColumnDbType { get; set; }

    public string? FixedDownBoxValue { get; set; }
}
