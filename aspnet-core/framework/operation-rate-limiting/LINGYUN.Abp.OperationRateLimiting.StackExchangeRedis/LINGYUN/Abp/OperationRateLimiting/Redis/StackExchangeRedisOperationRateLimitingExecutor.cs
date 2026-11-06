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
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Shared Redis connection and Lua script runner used by the Redis rules and state accessor.
/// </summary>
public class StackExchangeRedisOperationRateLimitingExecutor : IDisposable, ISingletonDependency
{
    protected AbpOperationRateLimitingRedisOptions Options { get; }

    protected IVirtualFileProvider VirtualFileProvider { get; }

    public ILogger<StackExchangeRedisOperationRateLimitingExecutor> Logger { get; set; }

    private readonly ConcurrentDictionary<string, string> _scriptCache = new();
    private readonly SemaphoreSlim _connectionLock = new(initialCount: 1, maxCount: 1);

    private volatile ConnectionMultiplexer? _connection;
    private volatile IDatabase? _database;

    public StackExchangeRedisOperationRateLimitingExecutor(
        IOptions<AbpOperationRateLimitingRedisOptions> options,
        IVirtualFileProvider virtualFileProvider)
    {
        Options = options.Value;
        VirtualFileProvider = virtualFileProvider;
        Logger = NullLogger<StackExchangeRedisOperationRateLimitingExecutor>.Instance;
    }

    public virtual string NormalizeKey(string key)
    {
        return string.IsNullOrEmpty(Options.InstanceName) ? key : Options.InstanceName + key;
    }

    public virtual async Task KeyDeleteAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var database = await GetDatabaseAsync(cancellationToken);

        await database.KeyDeleteAsync(NormalizeKey(key));
    }

    public virtual async Task<RedisResult> EvaluateAsync(
        string scriptPath,
        string key,
        RedisValue[] values,
        CancellationToken cancellationToken)
    {
        var database = await GetDatabaseAsync(cancellationToken);
        var script = await GetScriptAsync(scriptPath, cancellationToken);

        return await database.ScriptEvaluateAsync(
            script,
            new RedisKey[] { NormalizeKey(key) },
            values);
    }

    public virtual async Task<string> GetScriptAsync(string scriptPath, CancellationToken cancellationToken)
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

    public virtual async Task<IDatabase> GetDatabaseAsync(CancellationToken cancellationToken)
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

    public virtual OperationRateLimitingStoreResult ParseResult(
        RedisResult result,
        OperationRateLimitingRuleContext context)
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
            ? OperationRateLimitingStoreResult.Allow(context.Rule.MaxCount, current, retryAfter, context.Rule.Duration)
            : OperationRateLimitingStoreResult.Deny(context.Rule.MaxCount, current, retryAfter, context.Rule.Duration);
    }

    public virtual void Dispose()
    {
        _connection?.Dispose();
        _connectionLock.Dispose();
    }
}
