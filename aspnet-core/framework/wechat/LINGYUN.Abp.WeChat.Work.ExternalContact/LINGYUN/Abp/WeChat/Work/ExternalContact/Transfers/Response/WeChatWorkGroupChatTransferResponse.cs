using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
/// <summary>
/// 分配离职成员的客户群响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92127" />
/// </remarks>
public class WeChatWorkGroupChatTransferResponse : WeChatWorkResponse
{
    /// <summary>
    /// 没能成功继承的群
    /// </summary>
    [NotNull]
    [JsonProperty("failed_chat_list")]
    [JsonPropertyName("failed_chat_list")]
    public GroupChatTransferFailed[] FailedChatList { get; set; }
}
