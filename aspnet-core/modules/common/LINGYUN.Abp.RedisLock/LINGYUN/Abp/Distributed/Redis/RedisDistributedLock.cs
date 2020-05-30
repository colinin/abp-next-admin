using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Polly;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Distributed.Redis
{
    [ExposeServices(typeof(IDistributedLock))]
    [Dependency(ServiceLifetime.Singleton, TryRegister = true)]
    public class RedisDistributedLock : IDistributedLock
    {
        public ILogger<RedisDistributedLock> Logger { protected get; set; }

        private volatile ConnectionMultiplexer _connection;
        private IDatabase _redis;

        private readonly RedisLockOptions _options;
        private readonly string _instance;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RedisDistributedLock(IOptions<RedisLockOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;

            _instance = _options.InstanceName ?? string.Empty;

            Logger = NullLogger<RedisDistributedLock>.Instance;
        }

        private void RegistenConnectionEvent(ConnectionMultiplexer connection)
        {
            if (connection != null)
            {
                connection.ConnectionFailed += OnConnectionFailed;
                connection.ConnectionRestored += OnConnectionRestored;
                connection.ErrorMessage += OnErrorMessage;
                connection.ConfigurationChanged += OnConfigurationChanged;
                connection.HashSlotMoved += OnHashSlotMoved;
                connection.InternalError += OnInternalError;
                connection.ConfigurationChangedBroadcast += OnConfigurationChangedBroadcast;
            }
        }

        public bool Lock(string lockKey, string lockValue, int lockSecond = 30)
        {
            Connect();
            return LockTakeSync(lockKey, lockValue, TimeSpan.FromSeconds(lockSecond));
        }

        public async Task<bool> LockAsync(string lockKey, string lockValue, int lockSecond = 30, CancellationToken token = default)
        {
            await ConnectAsync(token);
            return await LockTakeAsync(lockKey, lockValue, TimeSpan.FromSeconds(lockSecond));
        }

        public IDisposable Lock(string lockKey, int lockSecond = 30)
        {
            Connect();
            var redisLockToken = Environment.MachineName;
            var redisLockKey = _instance + lockKey;
            var lockResult = LockTakeSync(redisLockKey, redisLockToken, TimeSpan.FromSeconds(lockSecond));
            if (lockResult)
            {
                return new DisposeAction(() =>
                {
                    LockReleaseSync(redisLockKey, redisLockToken);
                });
            }
            Logger.LogWarning("Redis lock failed of key: {0}", redisLockKey);
            throw new DistributedLockException(redisLockKey);
        }

        public async Task<IDisposable> LockAsync(string lockKey, int lockSecond = 30, CancellationToken token = default(CancellationToken))
        {
            await ConnectAsync(token);
            var redisLockToken = Environment.MachineName;
            var redisLockKey = _instance + lockKey;
            var lockResult = await LockTakeAsync(redisLockKey, redisLockToken, TimeSpan.FromSeconds(lockSecond));

            if (lockResult)
            {
                return new DisposeAction(async () =>
                {
                    await LockReleaseAsync(redisLockKey, redisLockToken);
                });
            }
            Logger.LogWarning("Redis lock failed of key: {0}", redisLockKey);
            throw new DistributedLockException(redisLockKey);
        }

        public bool Release(string lockKey, string lockValue)
        {
            Connect();
            return LockReleaseSync(lockKey, lockValue);
        }

        public async Task<bool> ReleaseAsync(string lockKey, string lockValue, CancellationToken token = default)
        {
            await ConnectAsync(token);
            return await LockReleaseAsync(lockKey, lockValue);
        }
        /// <summary>
        /// 同步加锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        protected virtual bool LockTakeSync(RedisKey key, RedisValue value, TimeSpan expiry)
        {
            // 定义重试策略
            var policy = Policy
                .HandleResult<bool>(result => !result)
                .WaitAndRetry(
                retryCount: _options.FailedRetryCount,
                sleepDurationProvider: sleep => TimeSpan.FromMilliseconds(_options.FailedRetryInterval),
                onRetry: (result, timeSpan) =>
                {
                    Logger.LogWarning("Redis lock take failed, retry policy timeSpan:{0}", timeSpan.ToString());
                });
            // 加锁
            var lockResult = policy.Execute(() =>  _redis.LockTake(key, value, expiry));

            return lockResult;
        }
        /// <summary>
        /// 异步加锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        protected virtual async Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry)
        {
            // 定义重试策略
            var policy = Policy
                .HandleResult<bool>(result => !result)
                .WaitAndRetryAsync(
                retryCount: _options.FailedRetryCount,
                sleepDurationProvider: sleep => TimeSpan.FromMilliseconds(_options.FailedRetryInterval),
                onRetry: (result, timeSpan) =>
                {
                    Logger.LogWarning("Redis lock take failed, retry policy timeSpan:{0}", timeSpan.ToString());
                });
            // 加锁
            var lockResult = await policy.ExecuteAsync(async () =>
                await _redis.LockTakeAsync(key, value, expiry));

            return lockResult;
        }
        /// <summary>
        /// 同步释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool LockReleaseSync(RedisKey key, RedisValue value)
        {
            // 定义重试策略
            var policy = Policy
                .HandleResult<bool>(result => !result)
                .WaitAndRetry(
                retryCount: _options.FailedRetryCount,
                sleepDurationProvider: sleep => TimeSpan.FromMilliseconds(_options.FailedRetryInterval),
                onRetry: (result, timeSpan) =>
                {
                    Logger.LogWarning("Redis lock release failed, retry policy timeSpan:{0}", timeSpan.ToString());
                });
            // 释放锁
            var lockReleaseResult = policy.Execute(() => _redis.LockRelease(key, value));

            return lockReleaseResult;
        }
        /// <summary>
        /// 异步释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual async Task<bool> LockReleaseAsync(RedisKey key, RedisValue value)
        {
            // 定义重试策略
            var policy = Policy
                .HandleResult<bool>(result => !result)
                .WaitAndRetryAsync(
                retryCount: _options.FailedRetryCount,
                sleepDurationProvider: sleep => TimeSpan.FromMilliseconds(_options.FailedRetryInterval),
                onRetry: (result, timeSpan) =>
                {
                    Logger.LogWarning("Redis lock release failed, retry policy timeSpan:{0}", timeSpan.ToString());
                });
            // 释放锁
            var lockReleaseResult = await policy.ExecuteAsync(async () =>
                await _redis.LockReleaseAsync(key, value));

            return lockReleaseResult;
        }

        private void Connect()
        {
            if (_redis != null)
            {
                return;
            }

            _connectionLock.Wait();
            try
            {
                if (_redis == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                    RegistenConnectionEvent(_connection);
                    _redis = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }


        private async Task ConnectAsync(CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (_redis != null)
            {
                return;
            }

            await _connectionLock.WaitAsync(token);
            try
            {
                if (_redis == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration);
                    }
                    RegistenConnectionEvent(_connection);
                    _redis = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private void OnConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            Logger.LogInformation("Redis lock server master/slave changes");
        }

        private void OnInternalError(object sender, InternalErrorEventArgs e)
        {
            Logger.LogError("Redis lock internal error, origin:{0}, connectionType:{1}", 
                e.Origin, e.ConnectionType);
            Logger.LogError(e.Exception, "Redis lock internal error");

        }

        private void OnHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Logger.LogInformation("Redis lock configuration changed");
        }

        private void OnConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Logger.LogInformation("Redis lock configuration changed");
        }

        private void OnErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Logger.LogWarning("Redis lock error, message:{0}", e.Message);
        }

        private void OnConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Logger.LogWarning("Redis lock connection restored, failureType:{0}, connectionType:{1}",
                e.FailureType, e.ConnectionType);
        }

        private void OnConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Logger.LogError("Redis lock connection failed, failureType:{0}, connectionType:{1}", 
                e.FailureType, e.ConnectionType);
            Logger.LogError(e.Exception, "Redis lock connection failed");
        }
    }
}
