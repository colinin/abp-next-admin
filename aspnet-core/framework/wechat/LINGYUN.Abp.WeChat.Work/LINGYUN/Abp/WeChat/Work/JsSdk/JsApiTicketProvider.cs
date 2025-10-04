using LINGYUN.Abp.WeChat.Work.JsSdk.Models;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;
public class JsApiTicketProvider : IJsApiTicketProvider, ISingletonDependency
{
    public ILogger<JsApiTicketProvider> Logger { get; set; }

    protected IHttpClientFactory HttpClientFactory { get; }
    protected IDistributedCache<JsApiTicketInfoCacheItem> Cache { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public JsApiTicketProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider,
        IDistributedCache<JsApiTicketInfoCacheItem> cache)
    {
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
        HttpClientFactory = httpClientFactory;
        Cache = cache;

        Logger = NullLogger<JsApiTicketProvider>.Instance;
    }

    public async virtual Task<JsApiTicketInfo> GetAgentTicketInfoAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = nameof(GetAgentTicketInfoAsync);
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var cackeItem = await GetCacheItemAsync(
            cacheKey, 
            $"/cgi-bin/ticket/get?access_token={token.AccessToken}&type=agent_config", 
            cancellationToken);

        return new JsApiTicketInfo(cackeItem.Ticket, cackeItem.ExpiresIn);
    }

    public async virtual Task<JsApiTicketInfo> GetTicketInfoAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = nameof(GetTicketInfoAsync);
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var cackeItem = await GetCacheItemAsync(
            cacheKey,
            $"/cgi-bin/get_jsapi_ticket?access_token={token.AccessToken}", 
            cancellationToken);

        return new JsApiTicketInfo(cackeItem.Ticket, cackeItem.ExpiresIn);
    }

    public virtual JsApiSignatureData GenerateSignature(JsApiTicketInfo ticketInfo, string url, CancellationToken cancellationToken = default)
    {
        var nonce = JsApiTicketHelper.GenerateNonce();
        var timestamp = JsApiTicketHelper.GenerateTimestamp().ToString();
        var signature = JsApiTicketHelper.GenerateSignature(ticketInfo.Ticket, nonce, timestamp, url);

        return new JsApiSignatureData(nonce, timestamp, signature);
    }

    protected async virtual Task<JsApiTicketInfoCacheItem> GetCacheItemAsync(
        string cacheKey,
        string jsapiTicketUrl,
        CancellationToken cancellationToken = default)
    {
        var cacheItem = await Cache.GetAsync(cacheKey, token: cancellationToken);

        if (cacheItem != null)
        {
            Logger.LogDebug($"Found JsApiTicket in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"Not found JsApiTicket in the cache, getting from the httpClient: {cacheKey}");

        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetAsync(
            jsapiTicketUrl, 
            cancellationToken);
        var ticketInfoResponse = await response.DeserializeObjectAsync<JsApiTicketInfoResponse>();
        var ticketInfo = ticketInfoResponse.ToJsApiTicket();
        cacheItem = new JsApiTicketInfoCacheItem(ticketInfo.Ticket, ticketInfo.ExpiresIn);

        Logger.LogDebug($"Setting the cache item: {cacheKey}");

        var cacheOptions = new DistributedCacheEntryOptions
        {
            // 设置绝对过期时间为Token有效期剩余的二分钟
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ticketInfo.ExpiresIn - 100),
        };

        await Cache.SetAsync(cacheKey, cacheItem, cacheOptions, token: cancellationToken);

        Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

        return cacheItem;
    }
}
