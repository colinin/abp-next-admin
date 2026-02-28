using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
/// <summary>
/// 批量获取审批单号请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91816"/>
/// </remarks>
public class WeChatWorkGetApprovalInfoRequest : WeChatWorkRequest
{
    /// <summary>
    /// 审批单提交的时间范围，开始时间，UNix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("starttime")]
    [JsonPropertyName("starttime")]
    public long StartTime { get; set; }
    /// <summary>
    /// 审批单提交的时间范围，结束时间，Unix时间戳
    /// </summary>
    [NotNull]
    [JsonProperty("endtime")]
    [JsonPropertyName("endtime")]
    public long EndTime { get; set; }
    /// <summary>
    /// 分页查询游标，默认为空串，后续使用返回的new_next_cursor进行分页拉取
    /// </summary>
    [CanBeNull]
    [JsonProperty("new_cursor")]
    [JsonPropertyName("new_cursor")]
    public string? NewCursor { get; set; } = "";
    /// <summary>
    /// 一次请求拉取审批单数量，默认值为100，上限值为100。
    /// 若accesstoken为自建应用，仅允许获取在应用可见范围内申请人提交的表单，返回的sp_no_list个数可能和size不一致，开发者需用next_cursor判断表单记录是否拉取完
    /// </summary>
    [NotNull]
    [JsonProperty("size")]
    [JsonPropertyName("size")]
    public int Size { get; set; }
    /// <summary>
    /// 筛选条件，可对批量拉取的审批申请设置约束条件，支持设置多个条件
    /// </summary>
    [CanBeNull]
    [JsonProperty("filters")]
    [JsonPropertyName("filters")]
    public List<ApprovalInfoFilter>? Filters { get; set; }
    public WeChatWorkGetApprovalInfoRequest(
        long sartTime,
        long endTime,
        int size = 100,
        List<ApprovalInfoFilter>? filters = null)
    {
        StartTime = sartTime;
        EndTime = endTime;
        Size = Check.Range(size, nameof(size), 1, 100);

        Filters = filters;
    }
}
