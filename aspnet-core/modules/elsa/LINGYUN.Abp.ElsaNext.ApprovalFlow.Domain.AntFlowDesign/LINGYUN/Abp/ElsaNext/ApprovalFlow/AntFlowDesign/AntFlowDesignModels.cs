using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign;

/// <summary>
/// AntFlow node type identifiers (see the designer's <c>constant.js</c>).
/// </summary>
public static class AntFlowNodeKinds
{
    public const string Approval = "approval";

    public const string Condition = "condition";
}

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

/// <summary>
/// Reads <c>nodeProperty</c> whether the designer wrote it as an object or as a serialised JSON string.
/// </summary>
public sealed class AntFlowNodePropertyConverter : JsonConverter<AntFlowNodeProperty>
{
    public override AntFlowNodeProperty? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.String:
                var raw = reader.GetString();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    return new AntFlowNodeProperty();
                }

                try
                {
                    using var stringDocument = JsonDocument.Parse(raw);
                    return stringDocument.RootElement.ValueKind == JsonValueKind.Object
                        ? stringDocument.RootElement.Deserialize<AntFlowNodeProperty>(options) ?? new AntFlowNodeProperty()
                        : new AntFlowNodeProperty();
                }
                catch (JsonException)
                {
                    // An unparsable nodeProperty is treated as an empty node property so the rest of
                    // the design can still be compiled.
                    return new AntFlowNodeProperty();
                }
            case JsonTokenType.StartObject:
                using (var document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.Deserialize<AntFlowNodeProperty>(options) ?? new AntFlowNodeProperty();
                }
            default:
                reader.Skip();
                return new AntFlowNodeProperty();
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        AntFlowNodeProperty value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}

internal sealed class AntFlowFlexibleStringConverter : JsonConverter<string>
{
    public override string? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.Number:
                return reader.TryGetInt64(out var integer)
                    ? integer.ToString(CultureInfo.InvariantCulture)
                    : reader.GetDouble().ToString(CultureInfo.InvariantCulture);
            case JsonTokenType.True:
                return "true";
            case JsonTokenType.False:
                return "false";
            default:
                using (var document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.GetRawText();
                }
        }
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}

internal sealed class AntFlowFlexibleInt32Converter : JsonConverter<int>
{
    public override int Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out var number))
                {
                    return number;
                }
                if (reader.TryGetDouble(out var real))
                {
                    return (int)real;
                }
                break;
            case JsonTokenType.String:
                var text = reader.GetString();
                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                {
                    return parsed;
                }
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedReal))
                {
                    return (int)parsedReal;
                }
                break;
            case JsonTokenType.True:
                return 1;
            case JsonTokenType.False:
                return 0;
            case JsonTokenType.Null:
                return 0;
        }

        reader.Skip();
        return 0;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

internal sealed class AntFlowFlexibleBooleanConverter : JsonConverter<bool>
{
    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Number:
                return !reader.TryGetInt32(out var number) || number != 0;
            case JsonTokenType.String:
                var text = reader.GetString();
                if (bool.TryParse(text, out var boolean))
                {
                    return boolean;
                }
                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numeric))
                {
                    return numeric != 0;
                }
                return string.Equals(text, "且", StringComparison.Ordinal) ||
                       string.Equals(text, "and", StringComparison.OrdinalIgnoreCase);
            case JsonTokenType.Null:
                return false;
        }

        reader.Skip();
        return false;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}

/// <summary>
/// Serializer options for the AntFlow contract. AntFlow payloads are loosely typed in the
/// designer (numbers stored as strings and booleans stored as 0/1), so the options are lenient.
/// </summary>
public static class AntFlowJson
{
    public static readonly JsonSerializerOptions Options = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        options.Converters.Add(new AntFlowFlexibleStringConverter());
        options.Converters.Add(new AntFlowFlexibleInt32Converter());
        options.Converters.Add(new AntFlowFlexibleBooleanConverter());

        return options;
    }
}

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
