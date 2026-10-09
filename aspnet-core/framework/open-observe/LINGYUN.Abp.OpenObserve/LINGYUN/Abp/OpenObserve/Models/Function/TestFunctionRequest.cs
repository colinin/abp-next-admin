using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 测试函数请求
/// </summary>
/// <remarks>
/// POST /api/{org_id}/functions/test
/// 注意: 该接口未出现在 API 文档中, 来自实现源码
/// </remarks>
public class TestFunctionRequest
{
    /// <summary>
    /// 函数体
    /// </summary>
    [JsonPropertyName("function")]
    public string Function { get; set; } = default!;

    /// <summary>
    /// 用于测试的样例数据
    /// </summary>
    [JsonPropertyName("events")]
    public JsonNode[] Events { get; set; } = [];

    /// <summary>
    /// 函数语言
    /// </summary>
    /// <remarks>
    /// 不传时由服务端自动识别
    /// </remarks>
    [JsonPropertyName("trans_type")]
    public FunctionTransType? TransType { get; set; }
}
