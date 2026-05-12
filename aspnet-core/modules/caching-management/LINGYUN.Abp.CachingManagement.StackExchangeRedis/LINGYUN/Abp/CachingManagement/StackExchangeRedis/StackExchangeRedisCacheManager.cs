using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.CachingManagement.StackExchangeRedis;

[Dependency(ReplaceServices = true)]
public class StackExchangeRedisCacheManager : ICacheManager, ISingletonDependency
{
    private readonly static string AbsoluteExpirationKey;
    private readonly static string SlidingExpirationKey;

    private readonly static MethodInfo ConnectAsyncMethod;
    private readonly static MethodInfo MapMetadataMethod;
    private readonly static RedisValue[] HashMembersAbsoluteExpirationSlidingExpiration;

    protected AbpCachingManagementStackExchangeRedisOptions ManagementOptions { get; }
    protected RedisCacheOptions RedisCacheOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IClock Clock { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache DistributedCache { get; }
    protected AbpRedisCache RedisCache => DistributedCache.As<AbpRedisCache>();

    static StackExchangeRedisCacheManager()
    {
        var type = typeof(RedisCache);

        ConnectAsyncMethod = type.GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        MapMetadataMethod = type.GetMethod("MapMetadata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        AbsoluteExpirationKey = type.GetField("AbsoluteExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.ToString()!;
        SlidingExpirationKey = type.GetField("SlidingExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.ToString()!;

        HashMembersAbsoluteExpirationSlidingExpiration = [AbsoluteExpirationKey, SlidingExpirationKey];
    }

    public StackExchangeRedisCacheManager(
        IClock clock,
        ICurrentTenant currentTenant,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<RedisCacheOptions> redisCacheOptions,
        IOptions<AbpCachingManagementStackExchangeRedisOptions> managementOptions)
    {
        Clock = clock;
        CurrentTenant = currentTenant;
        DistributedCache = distributedCache;// distributedCache.As<AbpRedisCache>();
        CacheOptions = cacheOptions.Value;
        RedisCacheOptions = redisCacheOptions.Value;
        ManagementOptions = managementOptions.Value;
    }

    public async virtual Task<CackeKeysResponse> GetKeysAsync(GetCacheKeysRequest request, CancellationToken cancellationToken = default)
    {
        var cache = await ConnectAsync(cancellationToken);

        // 缓存键名规则: InstanceName + (t + TenantId)(CurrentTenant.IsAvailable) + CacheItemName + KeyPrefix + Key
        // 缓存键名规则: InstanceName + (c:)(!CurrentTenant.IsAvailable) + CacheItemName + KeyPrefix + Key

        var match = "*";
        // abp*
        if (!RedisCacheOptions.InstanceName.IsNullOrWhiteSpace())
        {
            match = RedisCacheOptions.InstanceName;
        }
        // abp*t:xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx*
        // abp*c:*
        if (CurrentTenant.IsAvailable)
        {
            match += "t:" + CurrentTenant.Id.ToString() + "*";
        }
        else
        {
            match += "c:*";
        }
        // app*abp*t:xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx*application*
        // app*abp*c:*application*
        if (!request.Prefix.IsNullOrWhiteSpace())
        {
            match += request.Prefix.EnsureEndsWith('*');
        }
        else if (!CacheOptions.KeyPrefix.IsNullOrWhiteSpace())
        {
            match += CacheOptions.KeyPrefix.EnsureEndsWith('*');
        }
        // if filter is Mailing:
        // app*abp*t:xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx*application*Mailing*
        // app*abp*c:*application*Mailing*
        if (!request.Filter.IsNullOrWhiteSpace())
        {
            match += request.Filter.EnsureStartsWith('*').EnsureEndsWith('*');
        }
        // scan 0 match * count 50000
        // redis有自定义的key排序,由传递的marker来确定下一次检索起始位

        var nextCursor = request.Marker ?? "0";
        var scanKeys = new List<string>();

        if (!request.Prefix.IsNullOrWhiteSpace() || !request.Filter.IsNullOrWhiteSpace())
        {
            // 用户传递过滤条件时, 需要进行多次检索, 设定最大次数限制
            var dept = 1;
            do
            {
                if (dept > ManagementOptions.MaxScanDept)
                {
                    break;
                }

                var scanArgs = new object[] { nextCursor, "MATCH", match, "COUNT", ManagementOptions.ScanCount };
                var scanResult = await cache.ExecuteAsync("SCAN", scanArgs);

                var results = (RedisResult[])scanResult;
                nextCursor = (string)results[0];
                scanKeys.AddRange((string[])results[1]);

                dept++;
            } while (nextCursor != "0");
        }
        else
        {
            var scanArgs = new object[] { nextCursor, "MATCH", match, "COUNT", ManagementOptions.ScanCount };
            var scanResult = await cache.ExecuteAsync("SCAN", scanArgs);
            var results = (RedisResult[])scanResult;

            // 第一个返回结果 下一次检索起始位 0复位
            // 第二个返回结果为key列表
            // https://redis.io/commands/scan/
            nextCursor = (string)results[0];
            scanKeys.AddRange((string[])results[1]);
        }

        return new CackeKeysResponse(
            nextCursor,
            scanKeys);
    }

    public async virtual Task<CacheValueResponse> GetValueAsync(string key, CancellationToken cancellationToken = default)
    {
        long size = 0;
        var values = new Dictionary<string, object>();

        var cache = await ConnectAsync(cancellationToken);

        // type RedisKey
        var type = await cache.KeyTypeAsync(key);
        // ttl RedisKey
        var ttl = await cache.KeyTimeToLiveAsync(key);

        switch (type)
        {
            case RedisType.Hash:
                // hlen RedisKey
                size = await cache.HashLengthAsync(key);
                // hscan RedisKey
                await foreach (var hvalue in cache.HashScanAsync(key))
                {
                    if (!hvalue.Name.IsNullOrEmpty)
                    {
                        values.Add(hvalue.Name.ToString(), hvalue.Value.IsNullOrEmpty ? "" : hvalue.Value.ToString());
                    }
                }
                break;
            case RedisType.String:
                // strlen RedisKey
                size = await cache.StringLengthAsync(key);
                // get RedisKey
                var svalue = await cache.StringGetAsync(key);
                values.Add("value", svalue.IsNullOrEmpty ? "" : svalue.ToString());
                break;
            case RedisType.List:
                // llen RedisKey
                size = await cache.ListLengthAsync(key);
                // lrange RedisKey
                var lvalues = await cache.ListRangeAsync(key);
                for (var lindex = 0; lindex < lvalues.Length; lindex++)
                {
                    if (!lvalues[lindex].IsNullOrEmpty)
                    {
                        values.Add($"index.{lindex}", lvalues[lindex].IsNullOrEmpty ? "" : lvalues[lindex].ToString());
                    }
                }
                break;
            default:
                throw new BusinessException("Abp.CachingManagement:01001")
                    .WithData("Type", type.ToString());
        }

        return new CacheValueResponse(
            type.ToString(),
            size,
            values,
            ttl);
    }

    public async virtual Task SetAsync(SetCacheRequest request, CancellationToken cancellationToken = default)
    {
        var cacheKey = request.Key;
        if (!RedisCacheOptions.InstanceName.IsNullOrWhiteSpace() && cacheKey.StartsWith(RedisCacheOptions.InstanceName))
        {
            cacheKey = cacheKey.Substring(RedisCacheOptions.InstanceName.Length);
        }

        var distributedCacheEntryOptions = await BuildDistributedCacheEntryOptions(
            request.Key, 
            request.SlidingExpiration,
            request.AbsoluteExpiration,
            cancellationToken);

        await RedisCache.SetAsync(
            cacheKey,
            Encoding.UTF8.GetBytes(request.Value),
            distributedCacheEntryOptions,
            cancellationToken);
    }

    public async virtual Task RefreshAsync(RefreshCacheRequest request, CancellationToken cancellationToken = default)
    {
        var cacheKey = request.Key;
        if (!RedisCacheOptions.InstanceName.IsNullOrWhiteSpace() && cacheKey.StartsWith(RedisCacheOptions.InstanceName))
        {
            cacheKey = cacheKey.Substring(RedisCacheOptions.InstanceName.Length);
        }
        if (request.AbsoluteExpiration.HasValue || request.SlidingExpiration.HasValue)
        {
            var value = await RedisCache.GetAsync(cacheKey, cancellationToken);

            var distributedCacheEntryOptions = await BuildDistributedCacheEntryOptions(
            request.Key,
            request.SlidingExpiration,
            request.AbsoluteExpiration,
            cancellationToken);

            await RedisCache.SetAsync(
                cacheKey, 
                value,
                distributedCacheEntryOptions,
                cancellationToken);

            return;
        }
        await RedisCache.RefreshAsync(cacheKey, cancellationToken);
    }

    public async virtual Task RemoveAsync(RemoveCacheRequest request, CancellationToken cancellationToken = default)
    {
        var cacheKeys = request.Keys.Select((cacheKey) =>
        {
            if (!RedisCacheOptions.InstanceName.IsNullOrWhiteSpace() && cacheKey.StartsWith(RedisCacheOptions.InstanceName))
            {
                return cacheKey.Substring(RedisCacheOptions.InstanceName.Length);
            }
            return cacheKey;
        });
        await RedisCache.RemoveManyAsync(cacheKeys, cancellationToken);
    }

    protected virtual ValueTask<IDatabase> ConnectAsync(CancellationToken token = default)
    {
        return (ValueTask<IDatabase>)ConnectAsyncMethod.Invoke(RedisCache, new object[] { token });
    }

    protected virtual void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
    {
        var parameters = new object[] { results, null, null };
        MapMetadataMethod.Invoke(this, parameters);

        absoluteExpiration = (DateTimeOffset?)parameters[1];
        slidingExpiration = (TimeSpan?)parameters[2];
    }

    protected async virtual Task<DistributedCacheEntryOptions> BuildDistributedCacheEntryOptions(
        string key,
        TimeSpan? sldexp,
        TimeSpan? absexp,
        CancellationToken cancellationToken = default)
    {
        if (!sldexp.HasValue || !absexp.HasValue)
        {
            var cache = await ConnectAsync(cancellationToken);

            var redisValue = await cache.HashGetAsync(key, HashMembersAbsoluteExpirationSlidingExpiration);
            MapMetadata(redisValue, out var absoluteExpiration, out var slidingExpiration);
            if (absoluteExpiration.HasValue)
            {
                absexp ??= absoluteExpiration.Value - Clock.Now;
            }
            if (slidingExpiration.HasValue)
            {
                sldexp ??= slidingExpiration;
            }
        }

        // Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache
        // L580-L583: 键过期时间取自滑动过期时间与绝对过期时间的最小值, 这里为了防止前端显示问题, 强制滑动过期时间不小于绝对过期时间...
        // (long)Math.Min((absoluteExpiration.Value - creationTime).TotalSeconds, options.SlidingExpiration.Value.TotalSeconds);

        if (sldexp < absexp)
        {
            sldexp = absexp;
        }

        return new DistributedCacheEntryOptions
        {
            SlidingExpiration = sldexp,
            AbsoluteExpirationRelativeToNow = absexp,
        };
    }
}
