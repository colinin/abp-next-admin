using LINGYUN.Abp.WeChat.Official.Account.Models;
using LINGYUN.Abp.WeChat.Token;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Official.Account;
public class ParametricQrCodeGenerator : IParametricQrCodeGenerator, ITransientDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpWeChatOfficialOptionsFactory OfficialOptionsFactory { get; }
    protected IWeChatTokenProvider WeChatTokenProvider { get; }
    protected IDistributedCache<TicketModelCacheItem> TicketModelCache { get; }
    public ParametricQrCodeGenerator(
        IHttpClientFactory httpClientFactory, 
        IWeChatTokenProvider weChatTokenProvider,
        AbpWeChatOfficialOptionsFactory officialOptionsFactory,
        IDistributedCache<TicketModelCacheItem> ticketModelCache)
    {
        TicketModelCache = ticketModelCache;
        HttpClientFactory = httpClientFactory;
        WeChatTokenProvider = weChatTokenProvider;
        OfficialOptionsFactory = officialOptionsFactory;
    }

    public async virtual Task<TicketModel> CreateTicketAsync(CreateTicketModel model, CancellationToken cancellationToken = default)
    {
        var cacheItem = await GetOrCreateTicketModelCacheItem(model, cancellationToken);

        return new TicketModel
        {
            ExpireSeconds = cacheItem.ExpireSeconds,
            Ticket = cacheItem.Ticket,
            Url = cacheItem.Url,
        };
    }

    public async virtual Task<Stream> ShowQrCodeAsync(string ticket, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(AbpWeChatOfficialConsts.HttpClient);
        var response = await client.GetAsync($"/cgi-bin/showqrcode?ticket={ticket}", cancellationToken);
        response.ThrowNotSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }

    protected async virtual Task<TicketModelCacheItem> GetOrCreateTicketModelCacheItem(CreateTicketModel model, CancellationToken cancellationToken = default)
    {
        var cacheKey = TicketModelCacheItem.CalculateCacheKey(model.ActionName, model.SceneInfo.GetKey());
        var cacheItem = await TicketModelCache.GetAsync(cacheKey, token: cancellationToken);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        var options = await OfficialOptionsFactory.CreateAsync();

        var token = await WeChatTokenProvider.GetTokenAsync(options.AppId, options.AppSecret, cancellationToken);

        var client = HttpClientFactory.CreateClient(AbpWeChatGlobalConsts.HttpClient);
        var response = await client.PostAsync(
            $"/cgi-bin/qrcode/create?access_token={token.AccessToken}",
            new StringContent(model.SerializeToJson()));
        response.ThrowNotSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var ticketModel = JsonConvert.DeserializeObject<TicketModel>(responseContent);

        cacheItem = new TicketModelCacheItem(ticketModel.Ticket, ticketModel.ExpireSeconds, ticketModel.Url);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            // 设置绝对过期时间为Token有效期剩余的二分钟
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ticketModel.ExpireSeconds)
        };

        await TicketModelCache.SetAsync(cacheKey, cacheItem, cacheOptions, token: cancellationToken);

        return cacheItem;
    }
}
