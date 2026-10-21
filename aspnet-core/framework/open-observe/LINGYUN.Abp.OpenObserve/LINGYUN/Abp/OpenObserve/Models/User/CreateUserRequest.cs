using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.User;

/// <summary>
/// 创建用户请求
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// admin / user
    /// </remarks>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}
