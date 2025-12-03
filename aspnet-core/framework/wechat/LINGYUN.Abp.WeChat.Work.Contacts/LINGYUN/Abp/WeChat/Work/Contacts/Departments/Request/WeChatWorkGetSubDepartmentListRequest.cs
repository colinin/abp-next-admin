using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
/// <summary>
/// 获取子部门ID列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95350" />
/// </remarks>
public class WeChatWorkGetSubDepartmentListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 部门id。获取指定部门及其下的子部门（以及子部门的子部门等等，递归）。 如果不填，默认获取全量组织架构
    /// </summary>
    [CanBeNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int? Id { get; }
    public WeChatWorkGetSubDepartmentListRequest(int? id = null)
    {
        Id = id;
    }
}
