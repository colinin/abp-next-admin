using LINGYUN.Abp.Aliyun.Settings;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Settings;
using OpenApiConfig = AlibabaCloud.OpenApiClient.Models.Config;
using StsClient = AlibabaCloud.SDK.Sts20150401.Client;

namespace LINGYUN.Abp.Aliyun;

/// <summary>
/// 阿里云通用客户端构建工厂
/// </summary>
/// <typeparam name="TClient"></typeparam>
public abstract class AliyunClientFactory<TClient>
{
    protected ISettingProvider SettingProvider { get; }
    protected IDistributedCache<AliyunBasicSessionCredentialsCacheItem> Cache { get; }
    public AliyunClientFactory(
        ISettingProvider settingProvider,
        IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache)
    {
        Cache = cache;
        SettingProvider = settingProvider;
    }

    public async virtual Task<TClient> CreateAsync()
    {
        var regionId = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RegionId);
        var accessKey = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeyId);
        var accessKeySecret = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeySecret);

        Check.NotNullOrWhiteSpace(regionId, AliyunSettingNames.Authorization.RegionId);
        Check.NotNullOrWhiteSpace(accessKey, AliyunSettingNames.Authorization.AccessKeyId);
        Check.NotNullOrWhiteSpace(accessKeySecret, AliyunSettingNames.Authorization.AccessKeySecret);

        if (await SettingProvider.IsTrueAsync(AliyunSettingNames.Authorization.UseSecurityTokenService))
        {
            var cacheItem = await GetCacheItemAsync(accessKey, accessKeySecret, regionId);

            return GetSecurityTokenClient(
                regionId, 
                cacheItem.AccessKeyId,
                cacheItem.AccessKeySecret, 
                cacheItem.SecurityToken,
                cacheItem.Expiration);
        }

        return GetClient(regionId, accessKey, accessKeySecret);
    }

    protected abstract TClient GetClient(
        string regionId,
        string accessKeyId, 
        string accessKeySecret);

    protected abstract TClient GetSecurityTokenClient(
        string regionId,
        string accessKeyId, 
        string accessKeySecret, 
        string securityToken, 
        DateTime? expiration = null);

    protected async virtual Task<AliyunBasicSessionCredentialsCacheItem> GetCacheItemAsync(string accessKeyId, string accessKeySecret, string regionId)
    {
        var cacheKey = $"{accessKeyId}:{accessKeySecret}".ToMd5();
        var cacheItem = await Cache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var roleArn = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RamRoleArn);
            var roleSession = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RoleSessionName);
            Check.NotNullOrWhiteSpace(roleArn, AliyunSettingNames.Authorization.RamRoleArn);

            var policy = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.Policy);
            var durationSeconds = await SettingProvider.GetAsync(AliyunSettingNames.Authorization.DurationSeconds, 3000);

            var stsClient = new StsClient(
                new OpenApiConfig
                {
                    RegionId = regionId,
                    AccessKeyId = accessKeyId,
                    AccessKeySecret = accessKeySecret,
                });

            var assumeRoleResponse = await stsClient.AssumeRoleAsync(
                new AlibabaCloud.SDK.Sts20150401.Models.AssumeRoleRequest
                {
                    RoleArn = roleArn,
                    RoleSessionName = roleSession,
                    DurationSeconds = durationSeconds,
                    Policy = policy.IsNullOrWhiteSpace() ? null : policy,
                });

            cacheItem = new AliyunBasicSessionCredentialsCacheItem(
                assumeRoleResponse.Body.Credentials.AccessKeyId,
                assumeRoleResponse.Body.Credentials.AccessKeySecret,
                assumeRoleResponse.Body.Credentials.SecurityToken);

            var expirationTimeSpan = TimeSpan.FromSeconds(durationSeconds - 10);
            if (DateTime.TryParse(assumeRoleResponse.Body.Credentials.Expiration, out var expiration))
            {
                cacheItem.Expiration = expiration;
                expirationTimeSpan = new TimeSpan(expiration.AddSeconds(-10).Ticks);
            }

            await Cache.SetAsync(
                cacheKey,
                cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTimeSpan,
                });
        }

        return cacheItem;
    }
}
/// <summary>
/// 阿里云通用客户端构建工厂
/// </summary>
/// <typeparam name="TClient">客户端类型</typeparam>
/// <typeparam name="TConfiguration">客户端参数类型</typeparam>
public abstract class AliyunClientFactory<TClient, TConfiguration> : AliyunClientFactory<TClient>
{
    public AliyunClientFactory(
        ISettingProvider settingProvider,
        IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache)
        : base(settingProvider, cache)
    {
    }

    public async virtual Task<TClient> CreateAsync(TConfiguration configuration)
    {
        var regionId = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RegionId);
        var accessKey = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeyId);
        var accessKeySecret = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.AccessKeySecret);

        Check.NotNullOrWhiteSpace(regionId, AliyunSettingNames.Authorization.RegionId);
        Check.NotNullOrWhiteSpace(accessKey, AliyunSettingNames.Authorization.AccessKeyId);
        Check.NotNullOrWhiteSpace(accessKeySecret, AliyunSettingNames.Authorization.AccessKeySecret);

        if (await SettingProvider.IsTrueAsync(AliyunSettingNames.Authorization.UseSecurityTokenService))
        {
            var cacheItem = await GetCacheItemAsync(accessKey, accessKeySecret, regionId);

            return GetSecurityTokenClient(
                configuration, 
                regionId, 
                cacheItem.AccessKeyId, 
                cacheItem.AccessKeySecret,
                cacheItem.SecurityToken,
                cacheItem.Expiration);
        }

        return GetClient(configuration, regionId, accessKey, accessKeySecret);
    }

    protected abstract TClient GetClient(
        TConfiguration configuration, 
        string regionId, 
        string accessKeyId, 
        string accessKeySecret);

    protected abstract TClient GetSecurityTokenClient(
        TConfiguration configuration,
        string regionId, 
        string accessKeyId, 
        string accessKeySecret,
        string securityToken,
        DateTime? expiration = null);
}
