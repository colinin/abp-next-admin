using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

/// <summary>
/// Redis implementation of <see cref="IOperationRateLimitingStateAccessor"/>.
/// Custom rule states are updated with a Lua compare-and-set loop.
/// </summary>
[Dependency(ReplaceServices = true)]
public class StackExchangeRedisOperationRateLimitingStateAccessor : IOperationRateLimitingStateAccessor, ITransientDependency
{
    protected const int MaxStateUpdateAttempts = 500;

    protected const string StateCasScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/state-cas.lua";
    protected const string StateCasDeleteScriptPath = "/LINGYUN/Abp/OperationRateLimiting/Redis/Lua/state-cas-delete.lua";

    protected StackExchangeRedisOperationRateLimitingExecutor Executor { get; }

    protected IJsonSerializer JsonSerializer { get; }

    public StackExchangeRedisOperationRateLimitingStateAccessor(
        StackExchangeRedisOperationRateLimitingExecutor executor,
        IJsonSerializer jsonSerializer)
    {
        Executor = executor;
        JsonSerializer = jsonSerializer;
    }

    public virtual async Task<TState?> GetStateAsync<TState>(
        string key,
        CancellationToken cancellationToken = default)
        where TState : class, new()
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var value = await Executor.StringGetAsync(key, cancellationToken);

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

        var ttlMs = (long)Math.Max(1, (expiration > TimeSpan.Zero ? expiration : TimeSpan.FromMinutes(5)).TotalMilliseconds);

        for (var attempt = 0; attempt < MaxStateUpdateAttempts; attempt++)
        {
            var currentValue = await Executor.StringGetAsync(key, cancellationToken);
            var current = currentValue.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<TState>(currentValue!);

            var updated = updater(current);

            if (updated == null)
            {
                var deleted = await Executor.EvaluateAsync(
                    StateCasDeleteScriptPath,
                    key,
                    new RedisValue[]
                    {
                        currentValue.IsNullOrEmpty ? string.Empty : (string)currentValue!,
                        currentValue.IsNullOrEmpty ? 0 : 1
                    },
                    cancellationToken);

                if ((long)deleted == 1)
                {
                    return null;
                }
            }
            else
            {
                var newValue = JsonSerializer.Serialize(updated);

                var written = await Executor.EvaluateAsync(
                    StateCasScriptPath,
                    key,
                    new RedisValue[]
                    {
                        currentValue.IsNullOrEmpty ? string.Empty : (string)currentValue!,
                        currentValue.IsNullOrEmpty ? 0 : 1,
                        newValue,
                        ttlMs
                    },
                    cancellationToken);

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

        await Executor.KeyDeleteAsync(key, cancellationToken);
    }
}
