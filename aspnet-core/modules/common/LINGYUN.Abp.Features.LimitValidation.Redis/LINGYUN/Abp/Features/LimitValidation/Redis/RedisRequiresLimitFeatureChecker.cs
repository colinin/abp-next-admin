using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    [DisableConventionalRegistration]
    public class RedisRequiresLimitFeatureChecker : IRequiresLimitFeatureChecker
    {
        private const string CHECK_LUA_SCRIPT = "/LINGYUN/Abp/Features/LimitValidation/Redis/Lua/check.lua";
        private const string PROCESS_LUA_SCRIPT = "/LINGYUN/Abp/Features/LimitValidation/Redis/Lua/process.lua";

        public ILogger<RedisRequiresLimitFeatureChecker> Logger { protected get; set; }

        private volatile ConnectionMultiplexer _connection;
        private volatile ConfigurationOptions _redisConfig;
        private IDatabaseAsync _redis;
        private IServer _server;

        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly IRedisLimitFeatureNamingNormalizer _featureNamingNormalizer;
        private readonly AbpRedisRequiresLimitFeatureOptions _options;
        private readonly string _instance;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RedisRequiresLimitFeatureChecker(
            IVirtualFileProvider virtualFileProvider,
            IRedisLimitFeatureNamingNormalizer featureNamingNormalizer,
            IOptions<AbpRedisRequiresLimitFeatureOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;
            _virtualFileProvider = virtualFileProvider;
            _featureNamingNormalizer = featureNamingNormalizer;

            _instance = _options.InstanceName ?? string.Empty;

            Logger = NullLogger<RedisRequiresLimitFeatureChecker>.Instance;
        }

        public virtual async Task<bool> CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            await ConnectAsync(cancellation);

            var result = await EvaluateAsync(CHECK_LUA_SCRIPT, context, cancellation);
            return result + 1 <= context.Limit;
        }

        public virtual async Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            await ConnectAsync(cancellation);
            
            await EvaluateAsync(PROCESS_LUA_SCRIPT, context, cancellation);
        }

        private async Task<int> EvaluateAsync(string luaScriptFilePath, RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            var luaScriptFile = _virtualFileProvider.GetFileInfo(luaScriptFilePath);
            using var luaScriptFileStream = luaScriptFile.CreateReadStream();
            var fileBytes = await luaScriptFileStream.GetAllBytesAsync(cancellation);

            var luaSha1 = fileBytes.Sha1();
            if (!await _server.ScriptExistsAsync(luaSha1))
            {
                var luaScript = Encoding.UTF8.GetString(fileBytes);
                luaSha1 = await _server.ScriptLoadAsync(luaScript);
            }

            var keys = new RedisKey[1] { NormalizeKey(context) };
            var values = new RedisValue[] { context.GetEffectTicks() };
            var result = await _redis.ScriptEvaluateAsync(luaSha1, keys, values);
            if (result.Type == ResultType.Error)
            {
                throw new AbpException($"Script evaluate error: {result}");
            }
            return (int)result;
        }

        private string NormalizeKey(RequiresLimitFeatureContext context)
        {
            return _featureNamingNormalizer.NormalizeFeatureName(_instance, context);
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
                        _redisConfig = _options.ConfigurationOptions;
                    }
                    else
                    {
                        _redisConfig = ConfigurationOptions.Parse(_options.Configuration);
                    }
                    _redisConfig.AllowAdmin = true;
                    _redisConfig.SetDefaultPorts();
                    _connection = await ConnectionMultiplexer.ConnectAsync(_redisConfig);
                    // fix: 无需关注redis连接事件
                    _redis = _connection.GetDatabase();
                    _server = _connection.GetServer(_redisConfig.EndPoints[0]);
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }
}
