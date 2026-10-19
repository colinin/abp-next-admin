using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models;

/// <summary>
/// OpenObserve 通用响应
/// </summary>
/// <remarks>
/// 多数写操作接口返回 {"code":200,"message":"..."}
/// </remarks>
public class OpenObserveCodeMessageResponse
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// 请求是否成功
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Code is >= 200 and < 300;
}
