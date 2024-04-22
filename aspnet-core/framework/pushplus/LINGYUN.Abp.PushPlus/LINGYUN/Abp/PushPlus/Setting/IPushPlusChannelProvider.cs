using LINGYUN.Abp.PushPlus.Channel;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Setting;
/// <summary>
/// 发送渠道接口
/// </summary>
public interface IPushPlusChannelProvider
{
    /// <summary>
    /// 获取默认发送渠道
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusChannel> GetDefaultChannelAsync(
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 修改默认发送渠道
    /// </summary>
    /// <param name="defaultChannel">默认渠道；wechat-微信公众号,mail-邮件,cp-企业微信应用,webhook-第三方webhook</param>
    /// <param name="defaultWebhook">渠道参数；webhook和cp渠道需要填写具体的webhook编号或自定义编码</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateDefaultChannelAsync(
        PushPlusChannelType defaultChannel,
        string defaultWebhook = "",
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 修改接收消息限制
    /// </summary>
    /// <param name="recevieLimit">接收消息限制；0-接收全部，1-不接收消息</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateRecevieLimitAsync(
        PushPlusChannelRecevieLimit recevieLimit,
        CancellationToken cancellationToken = default);
}
