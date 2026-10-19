using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 函数列表响应
/// </summary>
public class FunctionListResponse
{
    [JsonPropertyName("list")]
    public FunctionInfo[] List { get; set; } = [];
}
