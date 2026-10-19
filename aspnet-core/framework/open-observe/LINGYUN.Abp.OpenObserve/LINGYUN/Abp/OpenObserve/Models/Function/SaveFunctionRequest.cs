using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Function;

/// <summary>
/// 创建/更新函数请求
/// </summary>
public class SaveFunctionRequest
{
    /// <summary>
    /// 函数名称
    /// </summary>
    /// <remarks>
    /// 更新接口以路径参数为准
    /// </remarks>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 函数体
    /// </summary>
    [JsonPropertyName("function")]
    public string Function { get; set; } = default!;

    /// <summary>
    /// 函数参数
    /// </summary>
    [JsonPropertyName("params")]
    public string? Params { get; set; }

    /// <summary>
    /// 参数个数
    /// </summary>
    [JsonPropertyName("numArgs")]
    public int? NumArgs { get; set; }

    /// <summary>
    /// 函数语言
    /// </summary>
    [JsonPropertyName("transType")]
    public FunctionTransType? TransType { get; set; }

    /// <summary>
    /// 函数生效的数据流
    /// </summary>
    [JsonPropertyName("streams")]
    public FunctionStreamInfo[]? Streams { get; set; }

    /// <summary>
    /// 执行顺序
    /// </summary>
    /// <remarks>
    /// 文档示例包含该字段, 但当前实现并不接受, 保留以兼容文档调用方式
    /// </remarks>
    [JsonPropertyName("order")]
    public int? Order { get; set; }
}
