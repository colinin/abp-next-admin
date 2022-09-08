using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using LINGYUN.Abp.Aliyun.Settings;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun
{
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

                return GetSecurityTokenClient(regionId, cacheItem.AccessKeyId, cacheItem.AccessKeySecret, cacheItem.SecurityToken);
            }

            return GetClient(regionId, accessKey, accessKeySecret);
        }

        protected abstract TClient GetClient(string regionId, string accessKeyId, string accessKeySecret);

        protected abstract TClient GetSecurityTokenClient(string regionId, string accessKeyId, string accessKeySecret, string securityToken);

        protected async virtual Task<AliyunBasicSessionCredentialsCacheItem> GetCacheItemAsync(string accessKeyId, string accessKeySecret, string regionId)
        {
            var cacheItem = await Cache.GetAsync(AliyunBasicSessionCredentialsCacheItem.CacheKey);
            if (cacheItem == null)
            {
                var roleArn = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RamRoleArn);
                var roleSession = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RoleSessionName);
                Check.NotNullOrWhiteSpace(roleArn, AliyunSettingNames.Authorization.RamRoleArn);

                var policy = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.Policy);
                var durationSeconds = await SettingProvider.GetAsync(AliyunSettingNames.Authorization.DurationSeconds, 3000);

                var profile = DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret);
                var request = new AssumeRoleRequest
                {
                    AcceptFormat = FormatType.JSON,
                    RoleArn = roleArn,
                    RoleSessionName = roleSession,
                    DurationSeconds = durationSeconds,
                    Policy = policy.IsNullOrWhiteSpace() ? null : policy
                };

                var client = new DefaultAcsClient(profile);
                var response = client.GetAcsResponse(request);

                cacheItem = new AliyunBasicSessionCredentialsCacheItem(
                    response.Credentials.AccessKeyId,
                    response.Credentials.AccessKeySecret,
                    response.Credentials.SecurityToken);

                await Cache.SetAsync(
                    AliyunBasicSessionCredentialsCacheItem.CacheKey,
                    cacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(durationSeconds - 10)
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
    public abstract class AliyunClientFactory<TClient, TConfiguration>
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

                return GetSecurityTokenClient(configuration, regionId, cacheItem.AccessKeyId, cacheItem.AccessKeySecret, cacheItem.SecurityToken);
            }

            return GetClient(configuration, regionId, accessKey, accessKeySecret);
        }

        protected abstract TClient GetClient(TConfiguration configuration, string regionId, string accessKeyId, string accessKeySecret);

        protected abstract TClient GetSecurityTokenClient(TConfiguration configuration, string regionId, string accessKeyId, string accessKeySecret, string securityToken);

        protected async virtual Task<AliyunBasicSessionCredentialsCacheItem> GetCacheItemAsync(string accessKeyId, string accessKeySecret, string regionId)
        {
            var cacheItem = await Cache.GetAsync(AliyunBasicSessionCredentialsCacheItem.CacheKey);
            if (cacheItem == null)
            {
                var roleArn = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RamRoleArn);
                var roleSession = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.RoleSessionName);
                Check.NotNullOrWhiteSpace(roleArn, AliyunSettingNames.Authorization.RamRoleArn);

                var policy = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Authorization.Policy);
                var durationSeconds = await SettingProvider.GetAsync(AliyunSettingNames.Authorization.DurationSeconds, 3000);

                var profile = DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret);
                var request = new AssumeRoleRequest
                {
                    AcceptFormat = FormatType.JSON,
                    RoleArn = roleArn,
                    RoleSessionName = roleSession,
                    DurationSeconds = durationSeconds,
                    Policy = policy.IsNullOrWhiteSpace() ? null : policy
                };

                var client = new DefaultAcsClient(profile);
                var response = client.GetAcsResponse(request);

                cacheItem = new AliyunBasicSessionCredentialsCacheItem(
                    response.Credentials.AccessKeyId,
                    response.Credentials.AccessKeySecret,
                    response.Credentials.SecurityToken);

                await Cache.SetAsync(
                    AliyunBasicSessionCredentialsCacheItem.CacheKey,
                    cacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(durationSeconds - 10)
                    });
            }

            return cacheItem;
        }
    }
}
