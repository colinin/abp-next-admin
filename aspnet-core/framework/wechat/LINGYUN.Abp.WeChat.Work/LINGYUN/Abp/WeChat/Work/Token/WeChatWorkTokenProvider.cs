using LINGYUN.Abp.WeChat.Work.Settings;
using LINGYUN.Abp.WeChat.Work.Token.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Token;

public class WeChatWorkTokenProvider : WeChatWorkTokenProviderBase, IWeChatWorkTokenProvider, ISingletonDependency
{
    protected override string ProviderName => "WeChatWorkToken";
    public WeChatWorkTokenProvider(
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory,
        IDistributedCache<WeChatWorkTokenCacheItem> cache)
        : base(settingProvider, httpClientFactory, cache)
    {
    }

    public async virtual Task<WeChatWorkToken> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);
        var secret = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.Secret);

        return await GetTokenAsync(corpId, agentId, secret, cancellationToken);
    }

    public async virtual Task<WeChatWorkToken> GetTokenAsync(
        string corpId, 
        string agentId, 
        string secret,
        CancellationToken cancellationToken = default)
    {
        return (await InternalGetTokenAsync(corpId, agentId, secret, cancellationToken)).Token;
    }
}
