using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.User;

/// <summary>
/// 用户列表响应
/// </summary>
/// <remarks>
/// 实测响应为 {"data":[...]}, 文档示例为 {"list":[...]}, 两者都做兼容
/// </remarks>
public class UserListResponse
{
    /// <summary>
    /// 用户列表
    /// </summary>
    [JsonPropertyName("data")]
    public UserInfo[]? Data { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    [JsonPropertyName("list")]
    public UserInfo[]? List { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    [JsonIgnore]
    public UserInfo[] Users => Data ?? List ?? [];
}
