using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// 创建成员响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90195" />
/// </remarks>
public class WeChatWorkCreateMemberResponse : WeChatWorkResponse
{
    /// <summary>
    /// 因填写不存在的部门，新增的部门列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("created_department_list")]
    [JsonPropertyName("created_department_list")]
    public Department[]? CreatedDepartmentList { get; set; }
}
