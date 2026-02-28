using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms;
/// <summary>
/// 会议室管理
/// </summary>
public interface IWeChatWorkMeetingRoomProvider
{
    /// <summary>
    /// 添加会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93619#%E6%B7%BB%E5%8A%A0%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkCreateMeetingRoomResponse> CreateMeetingRoomAsync(
        WeChatWorkCreateMeetingRoomRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93619#%E6%9F%A5%E8%AF%A2%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetMeetingRoomListResponse> GetMeetingRoomListAsync(
        WeChatWorkGetMeetingRoomListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 编辑会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93619#%E7%BC%96%E8%BE%91%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> UpdateMeetingRoomAsync(
        WeChatWorkUpdateMeetingRoomRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93619#%E5%88%A0%E9%99%A4%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteMeetingRoomAsync(
        WeChatWorkDeleteMeetingRoomRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询会议室的预定信息
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E6%9F%A5%E8%AF%A2%E4%BC%9A%E8%AE%AE%E5%AE%A4%E7%9A%84%E9%A2%84%E5%AE%9A%E4%BF%A1%E6%81%AF" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetMeetingRoomBookingListResponse> GetMeetingRoomBookingListAsync(
        WeChatWorkGetMeetingRoomBookingListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 预定会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkBookMeetingRoomResponse> BookMeetingRoomAsync(
        WeChatWorkBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 通过日程预定会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%80%9A%E8%BF%87%E6%97%A5%E7%A8%8B%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkBookMeetingRoomByScheduleResponse> BookMeetingRoomByScheduleAsync(
        WeChatWorkBookMeetingRoomByScheduleRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 通过会议预定会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E9%80%9A%E8%BF%87%E4%BC%9A%E8%AE%AE%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkBookMeetingRoomByMeetingResponse> BookMeetingRoomByMeetingAsync(
        WeChatWorkBookMeetingRoomByMeetingRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 取消预定会议室
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E5%8F%96%E6%B6%88%E9%A2%84%E5%AE%9A%E4%BC%9A%E8%AE%AE%E5%AE%A4" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> CancelBookMeetingRoomAsync(
        WeChatWorkCancelBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 根据会议室预定ID查询预定详情
    /// </summary>
    /// <remarks>
    /// 详情见：<see href="https://developer.work.weixin.qq.com/document/path/93620#%E6%A0%B9%E6%8D%AE%E4%BC%9A%E8%AE%AE%E5%AE%A4%E9%A2%84%E5%AE%9Aid%E6%9F%A5%E8%AF%A2%E9%A2%84%E5%AE%9A%E8%AF%A6%E6%83%85" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetMeetingRoomBookResponse> GetMeetingRoomBookAsync(
        WeChatWorkGetMeetingRoomBookRequest request,
        CancellationToken cancellationToken = default);
}
