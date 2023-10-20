using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Message;

public interface IPushPlusMessageProvider
{
    /// <summary>
    /// 查询消息发送结果
    /// </summary>
    /// <param name="shortCode">消息短链码</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SendPushPlusMessageResult> GetSendResultAsync(
        string shortCode,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 消息列表
    /// </summary>
    /// <param name="current">当前所在分页数</param>
    /// <param name="pageSize">每页大小，最大值为50</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusPagedResponse<PushPlusMessage>> GetMessageListAsync(
        int current,
        int pageSize,
        CancellationToken cancellationToken = default); 
}
