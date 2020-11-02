using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Authorization
{
    public interface IWeChatTokenProvider
    {
        Task<WeChatToken> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}
