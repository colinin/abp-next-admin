using LINGYUN.Abp.WeChat.Work.Security.Models;
using LINGYUN.Abp.WeChat.Work.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Security;
public class WeChatWorkServerProvider : IWeChatWorkServerProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkServerProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatServerDomainModel> GetWeChatServerAsync(CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetServerDomainIpAsync(token.AccessToken, cancellationToken);
        var serverDomainResponse = await response.DeserializeObjectAsync<WeChatServerDomainResponse>();

        return serverDomainResponse.ToServerDomain();
    }
}
