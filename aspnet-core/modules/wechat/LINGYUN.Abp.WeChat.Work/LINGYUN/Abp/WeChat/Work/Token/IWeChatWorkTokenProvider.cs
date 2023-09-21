using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Token
{
    public interface IWeChatWorkTokenProvider
    {
        Task<WeChatWorkToken> GetTokenAsync(string agentId, CancellationToken cancellationToken = default);
        Task<WeChatWorkToken> GetTokenAsync(string corpId, string agentId, CancellationToken cancellationToken = default);
    }
}
