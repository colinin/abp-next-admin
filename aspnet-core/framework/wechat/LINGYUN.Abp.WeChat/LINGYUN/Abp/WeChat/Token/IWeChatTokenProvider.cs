using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Token
{
    public interface IWeChatTokenProvider
    {
        Task<WeChatToken> GetTokenAsync(string appId, string appSecret, CancellationToken cancellationToken = default);
    }
}
