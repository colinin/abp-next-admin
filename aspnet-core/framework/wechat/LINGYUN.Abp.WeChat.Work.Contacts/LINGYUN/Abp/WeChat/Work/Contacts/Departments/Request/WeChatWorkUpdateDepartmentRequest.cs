using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
/// <summary>
/// 更新部门请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90205" />
/// </remarks>
public class WeChatWorkUpdateDepartmentRequest : WeChatWorkRequest
{
    /// <summary>
    /// 部门id
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; }
    /// <summary>
    /// 部门名称。长度限制为1~64个UTF-8字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// 英文名称，需要在管理后台开启多语言支持才能生效。长度限制为1~64个字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("name_en")]
    [JsonPropertyName("name_en")]
    public string? NameEn { get; set; }
    /// <summary>
    /// 父部门id
    /// </summary>
    [CanBeNull]
    [JsonProperty("parentid")]
    [JsonPropertyName("parentid")]
    public int? ParentId { get; set; }
    /// <summary>
    /// 在父部门中的次序值。order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    public WeChatWorkUpdateDepartmentRequest(int id)
    {
        Id = id;
    }

    protected override void Validate()
    {
        Check.NotNullOrWhiteSpace(Name, nameof(Name), 64, 1);
        Check.Length(NameEn, nameof(NameEn), 64, 1);
    }
}
