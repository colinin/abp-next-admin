using LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules;
/// <summary>
/// 日程管理
/// </summary>
public interface IWeChatWorkScheduleProvider
{
    /// <summary>
    /// 创建日程
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93648" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkCreateScheduleResponse> CreateScheduleAsync(
        WeChatWorkCreateScheduleRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新日程
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97720" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkUpdateScheduleResponse> UpdateScheduleAsync(
        WeChatWorkUpdateScheduleRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 新增日程参与者
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97721" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> AddAttendeesAsync(
        WeChatWorkScheduleAddAttendeesRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除日程参与者
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97722" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteAttendeesAsync(
        WeChatWorkScheduleDeleteAttendeesRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取日历下的日程列表
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97723" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetScheduleListByCalendarResponse> GetScheduleListByCalendarAsync(
        WeChatWorkGetScheduleListByCalendarRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取日程详情
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97724" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetScheduleListResponse> GetScheduleListAsync(
        WeChatWorkGetScheduleListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 取消日程
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/97725" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteScheduleAsync(
        WeChatWorkDeleteScheduleRequest request,
        CancellationToken cancellationToken = default);
}
