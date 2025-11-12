using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
/// <summary>
/// 分配离职成员客户详情
/// </summary>
public class UnassignedTransferCustomer
{
    /// <summary>
    /// 客户的外部联系人userid
    /// </summary>
    [NotNull]
    [JsonProperty("external_userid")]
    [JsonPropertyName("external_userid")]
    public string ExternalUserid { get; set; }
    /// <summary>
    /// 对此客户进行分配的结果,0表示开始分配流程,待24小时后自动接替,并不代表最终分配成功
    /// </summary>
    [NotNull]
    [JsonProperty("errcode")]
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }
}
