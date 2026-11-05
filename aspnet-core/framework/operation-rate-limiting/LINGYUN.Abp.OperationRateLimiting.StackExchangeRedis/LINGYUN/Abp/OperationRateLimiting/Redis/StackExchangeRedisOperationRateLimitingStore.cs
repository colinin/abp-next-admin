using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Redis implementation of <see cref="IOperationRateLimitingStore"/>.
/// </summary>
/// <remarks>
/// Every built-in algorithm is executed by a single Lua script, so the read-modify-write of the
/// counter is atomic without a distributed lock. Custom algorithm states are updated with a
/// Lua compare-and-set loop.
/// </remarks>
public class StackExchangeRedisOperationRateLimitingStore : IOperationRateLimitingStore, IDisposable
{
    protected const int MaxStateUpdateAttempts = 500;

    protected const string FixedWindowScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/FixedWindow/fixed-window.lua";
    protected const string SlidingWindowScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/SlidingWindow/sliding-window.lua";
    protected const string TokenBucketScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/TokenBucket/token-bucket.lua";
    protected const string ConcurrencyScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/Concurrency/concurrency.lua";
    protected const string ConcurrencyReleaseScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/Concurrency/concurrency-release.lua";
    protected const string StateCasScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/state-cas.lua";
    protected const string StateCasDeleteScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/state-cas-delete.lua";

    protected AbpOperationRateLimitingRedisOptions Options { get; }

    protected IVirtualFileProvider VirtualFileProvider { get; }

    protected IJsonSerializer JsonSerializer { get; }

    public ILogger<StackExchangeRedisOperationRateLimitingStore> Logger { get; set; }

    private readonly ConcurrentDictionary<string, string> _scriptCache = new();
    private readonly SemaphoreSlim _connectionLock = new(initialCount: 1, maxCount: 1);

    private volatile ConnectionMultiplexer? _connection;
    private volatile IDatabase? _database;

    public StackExchangeRedisOperationRateLimitingStore(
        IOptions<AbpOperationRateLimitingRedisOptions> options,
        IVirtualFileProvider virtualFileProvider,
        IJsonSerializer jsonSerializer)
    {
        Options = options.Value;
        VirtualFileProvider = virtualFileProvider;
        JsonSerializer = jsonSerializer;
        Logger = NullLogger<StackExchangeRedisOperationRateLimitingStore>.Instance;
    }

    public virtual async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));
        Check.NotNullOrWhiteSpace(request.Key, nameof(request.Key));

        if (request.MaxCount <= 0)
        {
            return OperationRateLimitingStoreResult.Deny(0, 0, null, request.Duration, isPermanent: true);
        }

        var key = NormalizeKey(request.Key);
        var nowMs = request.Now.ToUnixTimeMilliseconds();
        var consume = request.Consume ? 1 : 0;
        RedisResult result;

        switch (request.Algorithm)
        {
            case OperationRateLimitingAlgorithms.FixedWindow:
            {
                var windowMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
                var windowStartMs = nowMs - nowMs % windowMs;
                var ttlMs = Math.Max(1, windowStartMs + windowMs - nowMs);

                result = await EvaluateAsync(
                    FixedWindowScriptPath,
                    key,
                    new RedisValue[]
                    {
                        request.MaxCount,
                        consume,
                        ttlMs,
                        ttlMs
                    },
                    cancellationToken);
                break;
            }

            case OperationRateLimitingAlgorithms.SlidingWindow:
            {
                var windowMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
                var ttlMs = windowMs + 1000;

                result = await EvaluateAsync(
                    SlidingWindowScriptPath,
                    key,
                    new RedisValue[]
                    {
                        nowMs,
                        windowMs,
                        request.MaxCount,
                        consume,
                        Guid.NewGuid().ToString("N"),
                        ttlMs
                    },
                    cancellationToken);
                break;
            }

            case OperationRateLimitingAlgorithms.TokenBucket:
            {
                var tokensPerPeriod = Math.Max(1, request.TokensPerPeriod);
                var periodMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);
                var ttlMs = (long)Math.Min(
                    periodMs * (Math.Ceiling((double)request.MaxCount / tokensPerPeriod) + 2),
                    (double)TimeSpan.FromDays(1).TotalMilliseconds);

                result = await EvaluateAsync(
                    TokenBucketScriptPath,
                    key,
                    new RedisValue[]
                    {
                        nowMs,
                        periodMs,
                        request.MaxCount,
                        tokensPerPeriod,
                        consume,
                        ttlMs
                    },
                    cancellationToken);
                break;
            }

            case OperationRateLimitingAlgorithms.Concurrency:
            {
                var leaseMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);

                result = await EvaluateAsync(
                    ConcurrencyScriptPath,
                    key,
                    new RedisValue[]
                    {
                        nowMs,
                        leaseMs,
                        request.MaxCount,
                        consume,
                        Guid.NewGuid().ToString("N"),
                        leaseMs * 2
                    },
                    cancellationToken);
                break;
            }

            default:
                throw new AbpException(
                    $"The Redis operation rate limiting store does not implement the algorithm '{request.Algorithm}'. " +
                    "Custom algorithms must use IOperationRateLimitingStore.UpdateStateAsync in their rule implementation.");
        }

        return ParseResult(result, request);
    }

    public virtual async Task ReleaseAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        if (request.Algorithm != OperationRateLimitingAlgorithms.Concurrency)
        {
            return;
        }

        var leaseMs = Math.Max(1L, (long)request.Duration.TotalMilliseconds);

        await EvaluateAsync(
            ConcurrencyReleaseScriptPath,
            NormalizeKey(request.Key),
            new RedisValue[]
            {
                request.Now.ToUnixTimeMilliseconds(),
                leaseMs
            },
            cancellationToken);
    }

    public virtual async Task<TState?> GetStateAsync<TState>(
        string key,
        CancellationToken cancellationToken = default)
        where TState : class, new()
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var database = await GetDatabaseAsync(cancellationToken);
        var value = await database.StringGetAsync(NormalizeKey(key));

        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<TState>(value!);
    }

    public virtual async Task<TState?> UpdateStateAsync<TState>(
        string key,
        Func<TState?, TState?> updater,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
        where TState : class, new()
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(updater, nameof(updater));

        var database = await GetDatabaseAsync(cancellationToken);
        var redisKey = NormalizeKey(key);
        var ttlMs = (long)Math.Max(1, (expiration > TimeSpan.Zero ? expiration : TimeSpan.FromMinutes(5)).TotalMilliseconds);

        for (var attempt = 0; attempt < MaxStateUpdateAttempts; attempt++)
        {
            var currentValue = await database.StringGetAsync(redisKey);
            var current = currentValue.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<TState>(currentValue!);

            var updated = updater(current);

            if (updated == null)
            {
                var deleted = await database.ScriptEvaluateAsync(
                    await GetScriptAsync(StateCasDeleteScriptPath, cancellationToken),
                    new RedisKey[] { redisKey },
                    new RedisValue[]
                    {
                        currentValue.IsNullOrEmpty ? string.Empty : (string)currentValue!,
                        currentValue.IsNullOrEmpty ? 0 : 1
                    });

                if ((long)deleted == 1)
                {
                    return null;
                }
            }
            else
            {
                var newValue = JsonSerializer.Serialize(updated);

                var written = await database.ScriptEvaluateAsync(
                    await GetScriptAsync(StateCasScriptPath, cancellationToken),
                    new RedisKey[] { redisKey },
                    new RedisValue[]
                    {
                        currentValue.IsNullOrEmpty ? string.Empty : (string)currentValue!,
                        currentValue.IsNullOrEmpty ? 0 : 1,
                        newValue,
                        ttlMs
                    });

                if ((long)written == 1)
                {
                    return updated;
                }
            }
        }

        throw new AbpException(
            $"Could not update the operation rate limiting state '{key}' after {MaxStateUpdateAttempts} attempts because of concurrent modifications.");
    }

    public virtual async Task ResetAsync(string key, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var database = await GetDatabaseAsync(cancellationToken);
        await database.KeyDeleteAsync(NormalizeKey(key));
    }

    public virtual void Dispose()
    {
        _connection?.Dispose();
        _connectionLock.Dispose();
    }

    protected virtual string NormalizeKey(string key)
    {
        return string.IsNullOrEmpty(Options.InstanceName) ? key : Options.InstanceName + key;
    }

    protected virtual OperationRateLimitingStoreResult ParseResult(
        RedisResult result,
        OperationRateLimitingStoreRequest request)
    {
        if (result.Resp2Type == ResultType.Error)
        {
            throw new AbpException($"Operation rate limiting Lua script failed: {result}");
        }

        var values = (RedisResult[]?)result;
        if (values == null || values.Length < 4)
        {
            throw new AbpException($"Unexpected result of the operation rate limiting Lua script: {result}");
        }

        var allowed = (long)values[0] == 1;
        var current = (int)(long)values[1];
        var retryMs = (long)values[3];
        var retryAfter = retryMs > 0 ? TimeSpan.FromMilliseconds(retryMs) : (TimeSpan?)null;

        return allowed
            ? OperationRateLimitingStoreResult.Allow(request.MaxCount, current, retryAfter, request.Duration)
            : OperationRateLimitingStoreResult.Deny(request.MaxCount, current, retryAfter, request.Duration);
    }

    protected virtual async Task<RedisResult> EvaluateAsync(
        string scriptPath,
        string key,
        RedisValue[] values,
        CancellationToken cancellationToken)
    {
        var database = await GetDatabaseAsync(cancellationToken);
        var script = await GetScriptAsync(scriptPath, cancellationToken);

        return await database.ScriptEvaluateAsync(
            script,
            new RedisKey[] { key },
            values);
    }

    protected virtual async Task<string> GetScriptAsync(string scriptPath, CancellationToken cancellationToken)
    {
        if (_scriptCache.TryGetValue(scriptPath, out var cached))
        {
            return cached;
        }

        var file = VirtualFileProvider.GetFileInfo(scriptPath);
        if (!file.Exists)
        {
            throw new AbpException($"Could not find the operation rate limiting Lua script: {scriptPath}");
        }

        using var stream = file.CreateReadStream();
        using var reader = new StreamReader(stream);
        var script = await reader.ReadToEndAsync(cancellationToken);

        _scriptCache[scriptPath] = script;

        return script;
    }

    protected virtual async Task<IDatabase> GetDatabaseAsync(CancellationToken cancellationToken)
    {
        var database = _database;
        if (database != null)
        {
            return database;
        }

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_database == null)
            {
                var configuration = Options.ConfigurationOptions ?? ConfigurationOptions.Parse(Options.Configuration);
                configuration.SetDefaultPorts();

                _connection = await ConnectionMultiplexer.ConnectAsync(configuration);
                _database = _connection.GetDatabase();
            }

            return _database!;
        }
        finally
        {
            _connectionLock.Release();
        }
    }
}
