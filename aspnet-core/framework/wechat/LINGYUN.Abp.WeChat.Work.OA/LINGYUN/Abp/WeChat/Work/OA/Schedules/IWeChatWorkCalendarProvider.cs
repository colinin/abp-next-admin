using LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules;
/// <summary>
/// 日历管理
/// </summary>
public interface IWeChatWorkCalendarProvider
{
    /// <summary>
    /// 创建日历
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93647" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkCreateCalendarResponse> CreateCalendarAsync(
        WeChatWorkCreateCalendarRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新日历
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97716" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkUpdateCalendarResponse> UpdateCalendarAsync(
        WeChatWorkUpdateCalendarRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取日历详情
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97717" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetCalendarListResponse> GetCalendarListAsync(
        WeChatWorkGetCalendarListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除日历
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97718" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteCalendarAsync(
        WeChatWorkDeleteCalendarRequest request,
        CancellationToken cancellationToken = default);
}
