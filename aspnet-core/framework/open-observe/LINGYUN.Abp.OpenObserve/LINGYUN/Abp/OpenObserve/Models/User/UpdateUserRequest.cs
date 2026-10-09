using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.User;

/// <summary>
/// 更新用户请求
/// </summary>
public class UpdateUserRequest
{
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// 旧密码
    /// </summary>
    /// <remarks>
    /// 不修改密码时不要传值
    /// </remarks>
    [JsonPropertyName("old_password")]
    public string? OldPassword { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    /// <remarks>
    /// 不修改密码时不要传值
    /// </remarks>
    [JsonPropertyName("new_password")]
    public string? NewPassword { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}
