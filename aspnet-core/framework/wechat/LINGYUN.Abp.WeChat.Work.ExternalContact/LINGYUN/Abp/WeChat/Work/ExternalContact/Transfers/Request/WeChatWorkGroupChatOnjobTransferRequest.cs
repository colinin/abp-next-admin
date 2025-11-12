using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
/// <summary>
/// 分配在职成员的客户群请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95703" />
/// </remarks>
public class WeChatWorkGroupChatOnjobTransferRequest : WeChatWorkRequest
{
    /// <summary>
    /// 新群主ID
    /// </summary>
    [NotNull]
    [JsonProperty("new_owner")]
    [JsonPropertyName("new_owner")]
    public string NewOwner { get; }
    /// <summary>
    /// 需要转群主的客户群ID列表。取值范围： 1 ~ 100
    /// </summary>
    [NotNull]
    [JsonProperty("chat_id_list")]
    [JsonPropertyName("chat_id_list")]
    public string[] ChatIdList { get; }
    public WeChatWorkGroupChatOnjobTransferRequest(string newOwner, string[] chatIdList)
    {
        Check.NotNullOrWhiteSpace(newOwner, nameof(newOwner));
        Check.NotNullOrEmpty(chatIdList, nameof(chatIdList));

        if (chatIdList.Length < 1 || chatIdList.Length > 100)
        {
            throw new ArgumentException("The list of customer group ids that need to be transferred to the group owner must have more than 1 item or less than 100 items");
        }

        NewOwner = newOwner;
        ChatIdList = chatIdList;
    }
}
