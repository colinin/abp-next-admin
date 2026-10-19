using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 测试函数结果
/// </summary>
public class TestFunctionResult
{
    /// <summary>
    /// 处理消息
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// 处理后的数据
    /// </summary>
    [JsonPropertyName("event")]
    public JsonNode? Event { get; set; }
}
