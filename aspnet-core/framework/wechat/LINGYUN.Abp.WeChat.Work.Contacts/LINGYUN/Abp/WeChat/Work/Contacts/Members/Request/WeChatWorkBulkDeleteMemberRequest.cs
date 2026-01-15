using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
/// <summary>
/// 批量删除成员请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90199" />
/// </remarks>
public class WeChatWorkBulkDeleteMemberRequest : WeChatWorkRequest
{
    /// <summary>
    /// 成员UserID列表。对应管理端的账号。最多支持200个。若存在无效UserID，直接返回错误
    /// </summary>
    [NotNull]
    [JsonProperty("useridlist")]
    [JsonPropertyName("useridlist")]
    public List<string> UserIdList {  get; }
    public WeChatWorkBulkDeleteMemberRequest() :this([])
    {
    }

    public WeChatWorkBulkDeleteMemberRequest(List<string> userIdList)
    {
        UserIdList = userIdList;
    }

    protected override void Validate()
    {
        if (UserIdList.Count > 100)
        {
            throw new ArgumentException("The maximum number of member ids that can be deleted simultaneously is only 200!");
        }
    }
}
