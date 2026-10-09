using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 测试函数响应
/// </summary>
public class TestFunctionResponse
{
    [JsonPropertyName("results")]
    public TestFunctionResult[] Results { get; set; } = [];
}
