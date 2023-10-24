using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.Token;

public interface IWxPusherTokenProvider
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
}
