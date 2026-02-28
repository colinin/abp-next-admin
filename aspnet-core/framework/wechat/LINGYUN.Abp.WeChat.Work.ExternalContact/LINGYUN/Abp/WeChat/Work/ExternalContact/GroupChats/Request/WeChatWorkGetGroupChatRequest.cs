using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
/// <summary>
/// 获取客户群详情
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92122" />
/// </remarks>
public class WeChatWorkGetGroupChatRequest : WeChatWorkRequest
{
    /// <summary>
    /// 客户群ID
    /// </summary>
    [NotNull]
    [JsonProperty("chat_id")]
    [JsonPropertyName("chat_id")]
    public string ChatId { get; }
    /// <summary>
    /// 是否需要返回群成员的名字group_chat.member_list.name。0-不返回；1-返回。默认不返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("need_name")]
    [JsonPropertyName("need_name")]
    public int? NeedName { get; }
    public WeChatWorkGetGroupChatRequest(string chatId, bool needName = false)
    {
        Check.NotNullOrWhiteSpace(chatId, nameof(chatId));

        ChatId = chatId;
        NeedName = needName ? 1 : 0;
    }
}
