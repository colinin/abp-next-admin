using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// 获取成员ID列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/96067" />
/// </remarks>
public class WeChatWorkGetUserIdListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 用户-部门关系列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("next_cursor")]
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
    /// <summary>
    /// 分页游标，下次请求时填写以获取之后分页的记录。如果该字段返回空则表示已没有更多数据
    /// </summary>
    [NotNull]
    [JsonProperty("dept_user")]
    [JsonPropertyName("dept_user")]
    public DepartmentUser[] DepartmentUser { get; set; }
}
