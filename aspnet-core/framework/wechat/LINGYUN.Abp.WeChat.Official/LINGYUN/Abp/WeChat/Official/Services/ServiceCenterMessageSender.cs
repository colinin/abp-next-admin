using LINGYUN.Abp.WeChat.Official.Services.Models;
using LINGYUN.Abp.WeChat.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Official.Services;
public class ServiceCenterMessageSender : IServiceCenterMessageSender, ITransientDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpWeChatOfficialOptionsFactory OfficialOptionsFactory { get; }
    protected IWeChatTokenProvider WeChatTokenProvider { get; }
    public ServiceCenterMessageSender(
        IHttpClientFactory httpClientFactory,
        IWeChatTokenProvider weChatTokenProvider,
        AbpWeChatOfficialOptionsFactory officialOptionsFactory)
    {
        HttpClientFactory = httpClientFactory;
        WeChatTokenProvider = weChatTokenProvider;
        OfficialOptionsFactory = officialOptionsFactory;
    }

    public async virtual Task SendAsync(MessageModel message, CancellationToken cancellationToken = default)
    {
        var options = await OfficialOptionsFactory.CreateAsync();
        var token = await WeChatTokenProvider.GetTokenAsync(options.AppId, options.AppSecret, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatGlobalConsts.HttpClient);
        await client.PostAsync(
            $"cgi-bin/message/custom/send?access_token={token.AccessToken}",
            new StringContent(message.SerializeToJson()),
            cancellationToken);
    }
}
