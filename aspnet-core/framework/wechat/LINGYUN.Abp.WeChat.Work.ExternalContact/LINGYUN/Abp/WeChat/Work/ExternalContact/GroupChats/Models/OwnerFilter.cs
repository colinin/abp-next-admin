using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Models;
/// <summary>
/// 群主过滤
/// </summary>
public class OwnerFilter
{
    /// <summary>
    /// 用户ID列表。最多100个
    /// </summary>
    [NotNull]
    [JsonProperty("userid_list")]
    [JsonPropertyName("userid_list")]
    public string[] UserIdList { get; }
    public OwnerFilter(string[] userIdList)
    {
        Check.NotNullOrEmpty(userIdList, nameof(userIdList));

        if (userIdList.Length > 100)
        {
            throw new ArgumentException("The maximum number of parameters allowed for group owner filtering is only 100!");
        }

        UserIdList = userIdList;
    }
}
