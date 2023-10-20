using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.Messages;

public interface IWxPusherMessageProvider
{
    Task<WxPusherResult<int>> QueryMessageAsync(
        int messageId,
        CancellationToken cancellationToken = default);
}
