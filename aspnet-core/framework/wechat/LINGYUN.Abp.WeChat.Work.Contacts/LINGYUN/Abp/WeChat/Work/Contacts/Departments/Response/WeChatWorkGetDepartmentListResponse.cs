using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Response;
/// <summary>
/// 获取部门列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90208" />
/// </remarks>
public class WeChatWorkGetDepartmentListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 部门列表数据
    /// </summary>
    [NotNull]
    [JsonProperty("department")]
    [JsonPropertyName("department")]
    public DepartmentInfo[] Department { get; set; }
}
