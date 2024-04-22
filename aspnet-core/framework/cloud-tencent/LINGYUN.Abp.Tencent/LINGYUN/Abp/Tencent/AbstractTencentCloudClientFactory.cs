﻿using LINGYUN.Abp.Tencent.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent;

public abstract class AbstractTencentCloudClientFactory<TClient>
{
    protected IMemoryCache ClientCache { get; }
    protected ISettingProvider SettingProvider { get; }

    public AbstractTencentCloudClientFactory(
        IMemoryCache clientCache,
        ISettingProvider settingProvider)
    {
        ClientCache = clientCache;
        SettingProvider = settingProvider;
    }

    public async virtual Task<TClient> CreateAsync()
    {
        var clientCacheItem = await GetClientCacheItemAsync();

        return CreateClient(clientCacheItem);
    }

    protected abstract TClient CreateClient(TencentCloudClientCacheItem cloudCache);

    protected async virtual Task<TencentCloudClientCacheItem> GetClientCacheItemAsync()
    {
        return await ClientCache.GetOrCreateAsync(
            TencentCloudClientCacheItem.CalculateCacheKey("client-options"),
            async (cache) =>
            {
                var secretId = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.SecretId);
                var secretKey = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.SecretKey);
                var endpoint = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.EndPoint);
                var durationSecond = await SettingProvider.GetAsync(TencentCloudSettingNames.DurationSecond, 3600);

                Check.NotNullOrWhiteSpace(secretId, TencentCloudSettingNames.SecretId);
                Check.NotNullOrWhiteSpace(secretKey, TencentCloudSettingNames.SecretKey);


                var method = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.Connection.HttpMethod);
                var webProxy = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.Connection.WebProxy);
                var timeout = await SettingProvider.GetAsync(TencentCloudSettingNames.Connection.Timeout, 60);

                cache.SetAbsoluteExpiration(TimeSpan.FromSeconds(durationSecond));

                return new TencentCloudClientCacheItem
                {
                    SecretId = secretId,
                    SecretKey = secretKey,
                    // 连接区域
                    EndPoint = endpoint,
                    DurationSecond = durationSecond,
                    HttpMethod = method,
                    WebProxy = webProxy,
                    Timeout = timeout,
                };
            });
    }
}

public abstract class AbstractTencentCloudClientFactory<TClient, TConfiguration>
{
    protected IMemoryCache ClientCache { get; }
    protected ISettingProvider SettingProvider { get; }

    public AbstractTencentCloudClientFactory(
        IMemoryCache clientCache,
        ISettingProvider settingProvider)
    {
        ClientCache = clientCache;
        SettingProvider = settingProvider;
    }

    public async virtual Task<TClient> CreateAsync(TConfiguration configuration)
    {
        var clientCacheItem = await GetClientCacheItemAsync();

        return CreateClient(configuration, clientCacheItem);
    }

    protected abstract TClient CreateClient(TConfiguration configuration, TencentCloudClientCacheItem cloudCache);

    protected async virtual Task<TencentCloudClientCacheItem> GetClientCacheItemAsync()
    {
        return await ClientCache.GetOrCreateAsync(
            TencentCloudClientCacheItem.CalculateCacheKey("client-options"),
            async (cache) =>
            {
                var secretId = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.SecretId);
                var secretKey = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.SecretKey);
                var endpoint = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.EndPoint);
                var durationSecond = await SettingProvider.GetAsync(TencentCloudSettingNames.DurationSecond, 3600);

                Check.NotNullOrWhiteSpace(secretId, TencentCloudSettingNames.SecretId);
                Check.NotNullOrWhiteSpace(secretKey, TencentCloudSettingNames.SecretKey);


                var method = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.Connection.HttpMethod);
                var webProxy = await SettingProvider.GetOrNullAsync(TencentCloudSettingNames.Connection.WebProxy);
                var timeout = await SettingProvider.GetAsync(TencentCloudSettingNames.Connection.Timeout, 60);

                cache.SetAbsoluteExpiration(TimeSpan.FromSeconds(durationSecond));

                return new TencentCloudClientCacheItem
                {
                    SecretId = secretId,
                    SecretKey = secretKey,
                    // 连接区域
                    EndPoint = endpoint,
                    DurationSecond = durationSecond,
                    HttpMethod = method,
                    WebProxy = webProxy,
                    Timeout = timeout,
                };
            });
    }
}
