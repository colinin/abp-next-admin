using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 筛选条件
/// </summary>
public class ApprovalInfoFilter
{
    /// <summary>
    /// 筛选类型
    /// </summary>
    /// <remarks>
    /// 包括如下值:<br />
    /// template_id - 模板类型/模板id;<br />
    /// creator - 申请人;<br />
    /// department - 审批单提单者所在部门;<br />
    /// sp_status - 审批状态;<br />
    /// record_type - 审批单类型属性;<br />
    /// <br />
    /// 注意:<br />
    /// 1、仅“部门”支持同时配置多个筛选条件。<br />
    /// 2、不同类型的筛选条件之间为“与”的关系，同类型筛选条件之间为“或”的关系<br />
    /// 3、record_type筛选类型仅支持2021/05/31以后新提交的审批单，历史单不支持表单类型属性过滤<br />
    /// </remarks>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    /// <summary>
    /// 筛选值
    /// </summary>
    /// <remarks>
    /// 对应值:<br />
    /// template_id-模板id;<br />
    /// creator-申请人userid;<br />
    /// department-所在部门id;<br />
    /// sp_status-审批单状态（1-审批中;2-已通过;3-已驳回;4-已撤销;6-通过后撤销;7-已删除;10-已支付）;<br />
    /// record_type-审批单类型属性（1-请假;2-打卡补卡;3-出差;4-外出;5-加班; 6- 调班;7-会议室预定;8-退款审批;9-红包报销审批）<br />
    /// </remarks>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
    public ApprovalInfoFilter(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
