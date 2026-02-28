using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
/// <summary>
/// 分配离职成员的客户响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94081" />
/// </remarks>
public class WeChatWorkResignedTransferCustomerResponse : WeChatWorkResponse
{
    /// <summary>
    /// 分配离职成员客户结果
    /// </summary>
    [NotNull]
    [JsonProperty("customer")]
    [JsonPropertyName("customer")]
    public UnassignedTransferCustomer[] Customer { get; set; }
}
