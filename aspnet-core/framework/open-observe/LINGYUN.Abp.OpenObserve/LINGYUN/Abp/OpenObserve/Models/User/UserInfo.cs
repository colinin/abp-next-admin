using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.User;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// 名
    /// </summary>
    /// <remarks>
    /// 文档的响应示例误写为 fist_name, 此处按字段表使用 first_name, 并通过扩展属性兜底
    /// </remarks>
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// admin / user / root
    /// </remarks>
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    /// <summary>
    /// 是否外部用户
    /// </summary>
    [JsonPropertyName("is_external")]
    public bool? IsExternal { get; set; }

    /// <summary>
    /// 创建时间(微秒)
    /// </summary>
    [JsonPropertyName("created_at")]
    public long? CreatedAt { get; set; }

    /// <summary>
    /// 未在模型中声明的字段
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraProperties { get; set; }
}
