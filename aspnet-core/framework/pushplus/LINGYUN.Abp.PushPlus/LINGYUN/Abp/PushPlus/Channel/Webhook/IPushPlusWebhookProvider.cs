using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Channel.Webhook;
/// <summary>
/// Webhook配置接口
/// </summary>
public interface IPushPlusWebhookProvider
{
    /// <summary>
    /// 获取webhook列表
    /// </summary>
    /// <param name="current">当前所在分页数</param>
    /// <param name="pageSize">每页大小，最大值为50</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusPagedResponse<PushPlusWebhook>> GetWebhookListAsync(
        int current = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// webhook详情
    /// </summary>
    /// <param name="webhookId">webhook编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusWebhook> GetWebhookAsync(
        int webhookId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 新增webhook
    /// </summary>
    /// <param name="webhookCode">webhook编码</param>
    /// <param name="webhookName">webhook名称</param>
    /// <param name="webhookType">webhook类型；1-企业微信，2-钉钉，3-飞书，4-server酱</param>
    /// <param name="webhookUrl">调用的url地址</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CreateWebhookAsync(
        string webhookCode,
        string webhookName,
        PushPlusWebhookType webhookType,
        string webhookUrl,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 修改webhook配置
    /// </summary>
    /// <param name="id">webhook编号</param>
    /// <param name="webhookCode">webhook编码</param>
    /// <param name="webhookName">webhook名称</param>
    /// <param name="webhookType">webhook类型；1-企业微信，2-钉钉，3-飞书，4-server酱</param>
    /// <param name="webhookUrl">调用的url地址</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> UpdateWebhookAsync(
        int id,
        string webhookCode,
        string webhookName,
        PushPlusWebhookType webhookType,
        string webhookUrl,
        CancellationToken cancellationToken = default);
}
