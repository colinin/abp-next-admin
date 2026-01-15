using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
/// <summary>
/// 获取单个部门详情请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90207" />
/// </remarks>
public class WeChatWorkGetDepartmentRequest : WeChatWorkRequest
{
    /// <summary>
    /// 部门id
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; }
    public WeChatWorkGetDepartmentRequest(int id)
    {
        Id = id;
    }
}
