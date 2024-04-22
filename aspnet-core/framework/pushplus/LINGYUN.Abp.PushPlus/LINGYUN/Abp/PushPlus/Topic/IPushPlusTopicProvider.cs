using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Topic;
/// <summary>
/// 群组接口
/// </summary>
public interface IPushPlusTopicProvider
{
    /// <summary>
    /// 获取群组列表
    /// </summary>
    /// <param name="current">当前所在分页数</param>
    /// <param name="pageSize">每页大小，最大值为50</param>
    /// <param name="topicType">群组类型;0-我创建的，1-我加入的</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusPagedResponse<PushPlusTopic>> GetTopicListAsync(
        int current,
        int pageSize,
        PushPlusTopicType topicType = PushPlusTopicType.Create,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取我创建的群组详情
    /// </summary>
    /// <param name="topicId">群组编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusTopicProfile> GetTopicProfileAsync(
        int topicId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取我加入的群详情
    /// </summary>
    /// <param name="topicId">群组编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusTopicForMe> GetTopicForMeProfileAsync(
        int topicId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 新增群组
    /// </summary>
    /// <param name="topicCode">群组编码</param>
    /// <param name="topicName">群组名称</param>
    /// <param name="contact">联系方式</param>
    /// <param name="introduction">群组简介</param>
    /// <param name="receiptMessage">加入后回复内容</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CreateTopicAsync(
        string topicCode,
        string topicName,
        string contact,
        string introduction,
        string receiptMessage = "",
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取群组二维码
    /// </summary>
    /// <param name="topicId">群组编号</param>
    /// <param name="forever">二维码类型；0-临时二维码，1-永久二维码</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusTopicQrCode> GetTopicQrCodeAsync(
        int topicId,
        PushPlusTopicQrCodeType forever = PushPlusTopicQrCodeType.Temporary,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 退出群组
    /// </summary>
    /// <param name="topicId">群组编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> QuitTopicAsync(
        int topicId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取群组内用户
    /// </summary>
    /// <param name="current">当前所在分页数</param>
    /// <param name="pageSize">每页大小，最大值为50</param>
    /// <param name="topicId">群组编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusPagedResponse<PushPlusTopicUser>> GetSubscriberListAsync(
        int current,
        int pageSize,
        int topicId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除群组内用户
    /// </summary>
    /// <param name="topicRelationId">用户编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> UnSubscriberAsync(
        int topicRelationId,
        CancellationToken cancellationToken = default);
}
