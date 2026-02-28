using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
/// <summary>
/// 创建部门请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90205" />
/// </remarks>
public class WeChatWorkCreateDepartmentRequest : WeChatWorkRequest
{
    /// <summary>
    /// 部门名称。同一个层级的部门名称不能重复。长度限制为1~64个UTF-8字符
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; }
    /// <summary>
    /// 英文名称。同一个层级的部门名称不能重复。需要在管理后台开启多语言支持才能生效。长度限制为1~64个字符
    /// </summary>
    [CanBeNull]
    [JsonProperty("name_en")]
    [JsonPropertyName("name_en")]
    public string? NameEn { get; set; }
    /// <summary>
    /// 父部门id，32位整型
    /// </summary>
    [NotNull]
    [JsonProperty("parentid")]
    [JsonPropertyName("parentid")]
    public int ParentId { get; }
    /// <summary>
    /// 在父部门中的次序值。order值大的排序靠前。有效的值范围是[0, 2^32)
    /// </summary>
    [CanBeNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int? Order { get; set; }
    /// <summary>
    /// 部门id，32位整型，指定时必须大于1。若不填该参数，将自动生成id
    /// </summary>
    [CanBeNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    public WeChatWorkCreateDepartmentRequest(string name, int parentId = 0, string? nameEn = null, int? order = null, int? id = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name), 64, 1);
        Check.Length(nameEn, nameof(nameEn), 64, 1);

        Name = name;
        NameEn = nameEn;
        ParentId = parentId;
        Order = order;
        Id = id;
    }

    protected override void Validate()
    {
        Check.NotNullOrWhiteSpace(Name, nameof(Name), 64, 1);
        Check.Length(NameEn, nameof(NameEn), 64, 1);
    }
}
