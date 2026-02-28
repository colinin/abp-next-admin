using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Response;
/// <summary>
/// 获取子部门ID列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95350" />
/// </remarks>
public class WeChatWorkGetSubDepartmentListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 部门列表数据。
    /// </summary>
    [NotNull]
    [JsonProperty("department_id")]
    [JsonPropertyName("department_id")]
    public SubDepartment[] Department { get; set; }
}
